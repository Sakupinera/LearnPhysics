﻿using System;
using UnityEngine;
using System.Collections;

public class Rigid_Bunny : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;

        float m = 1;
        mass = 0;
        for (int i = 0; i < vertices.Length; i++)
        {
            mass += m;
            float diag = m * vertices[i].sqrMagnitude;
            I_ref[0, 0] += diag;
            I_ref[1, 1] += diag;
            I_ref[2, 2] += diag;
            I_ref[0, 0] -= m * vertices[i][0] * vertices[i][0];
            I_ref[0, 1] -= m * vertices[i][0] * vertices[i][1];
            I_ref[0, 2] -= m * vertices[i][0] * vertices[i][2];
            I_ref[1, 0] -= m * vertices[i][1] * vertices[i][0];
            I_ref[1, 1] -= m * vertices[i][1] * vertices[i][1];
            I_ref[1, 2] -= m * vertices[i][1] * vertices[i][2];
            I_ref[2, 0] -= m * vertices[i][2] * vertices[i][0];
            I_ref[2, 1] -= m * vertices[i][2] * vertices[i][1];
            I_ref[2, 2] -= m * vertices[i][2] * vertices[i][2];
        }
        I_ref[3, 3] = 1;
    }

    Matrix4x4 Get_Cross_Matrix(Vector3 a)
    {
        //Get the cross product matrix of vector a
        Matrix4x4 A = Matrix4x4.zero;
        A[0, 0] = 0;
        A[0, 1] = -a[2];
        A[0, 2] = a[1];
        A[1, 0] = a[2];
        A[1, 1] = 0;
        A[1, 2] = -a[0];
        A[2, 0] = -a[1];
        A[2, 1] = a[0];
        A[2, 2] = 0;
        A[3, 3] = 1;
        return A;
    }

    private Quaternion Add(Quaternion a, Quaternion b)
    {
        a.x += b.x;
        a.y += b.y;
        a.z += b.z;
        a.w += b.w;
        return a;
    }

    private Matrix4x4 Matrix_subtraction(Matrix4x4 a, Matrix4x4 b)
    {
        for (int i = 0; i < 4; ++i)
        {
            for (int j = 0; j < 4; ++j)
            {
                a[i, j] -= b[i, j];
            }
        }
        return a;
    }


    private Matrix4x4 Matrix_miltiply_float(Matrix4x4 a, float b)
    {
        for (int i = 0; i < 4; ++i)
        {
            for (int j = 0; j < 4; ++j)
            {
                a[i, j] *= b;
            }
        }
        return a;
    }

    // In this function, update v and w by the impulse due to the collision with
    //a plane <P, N>
    //P 为该平面上的一个点，N为该平面的法线
    void Collision_Impulse(Vector3 P, Vector3 N)
    {
        //1.获取物体的每一个顶点(局部坐标)
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;

        //2.得到每一个顶点的全局坐标旋转矩阵R,和平移向量
        Matrix4x4 R = Matrix4x4.Rotate(transform.rotation);  //旋转矩阵
        Vector3 T = transform.position;                      //平移向量

        Vector3 sum = new Vector3(0, 0, 0);                 //碰撞点
        int collisionNum = 0;                               //碰撞点数量


        for (int i = 0; i < vertices.Length; i++)
        {
            //3.计算每个顶点到该表面的距离d
            Vector3 r_i = vertices[i];
            Vector3 Rri = R.MultiplyVector(r_i);
            Vector3 x_i = T + Rri;
            float d = Vector3.Dot(x_i - P, N);
            if (d < 0.0f)//发生碰撞(只有当平面为无限大平面时才能这样判断，否则还要判断碰撞点是否在物体上)
            {
                //4.将该点移动到距离表面最近的点。?????
                //x_i -= d * N
                //5.判断物体是否仍向墙体内部运动
                Vector3 v_i = v + Vector3.Cross(w, Rri);
                float v_N_size = Vector3.Dot(v_i, N);
                if (v_N_size < 0.0f)
                {
                    sum += r_i;
                    collisionNum++;
                }
            }
        }
        //Debug.LogFormat("共有{0}个点", collisionNum);

        if (collisionNum == 0) return;
        Matrix4x4 I_rot = R * I_ref * Matrix4x4.Transpose(R);//惯性张量（全局）
        Matrix4x4 I_inverse = Matrix4x4.Inverse(I_rot);      //惯性张量的逆（全局）
        Vector3 r_collision = sum / collisionNum;                //虚拟碰撞点（局部坐标）
        Vector3 Rr_collision = R.MultiplyVector(r_collision);
        //Vector3 x_collision = T + Rr_collision;							 //虚拟碰撞点（全局坐标）
        Vector3 v_collision = v + Vector3.Cross(w, Rr_collision);
        //6.如果物体仍向墙体内部运动,修改为新速度
        Vector3 v_N = Vector3.Dot(v_collision, N) * N;
        Vector3 v_T = v_collision - v_N;
        Vector3 v_N_new = -1.0f * restitution * v_N;
        float a = Math.Max(1.0f - friction * (1.0f + restitution) * v_N.magnitude / v_T.magnitude, 0.0f);
        Vector3 v_T_new = a * v_T;
        Vector3 v_new = v_N_new + v_T_new;
        //7.通过新速度量计算冲量J
        Matrix4x4 Rri_star = Get_Cross_Matrix(Rr_collision);
        Matrix4x4 K = Matrix_subtraction(Matrix_miltiply_float(Matrix4x4.identity, 1.0f / mass),
                                        Rri_star * I_inverse * Rri_star);
        Vector3 J = K.inverse.MultiplyVector(v_new - v_collision);
        //8.计算dv,dw;
        v += 1.0f / mass * J;
        w += I_inverse.MultiplyVector(Vector3.Cross(Rr_collision, J));
		//9.通过冲量J改变整个物体的线速度和角速度
    }

    // Update is called once per frame
    void Update()
    {
        //Game Control
        if (Input.GetKey("r"))
        {
            transform.position = new Vector3(0, 0.6f, 0);
            restitution = 0.5f;
            launched = false;
        }
        if (Input.GetKey("l"))
        {
            v = new Vector3(5, 2, 0);
            launched = true;
        }

        if (launched)
        {
            // Part I: Update velocities
            v += dt * gravity;
            v *= linear_decay;
            w *= angular_decay;

            // Part II: Collision Impulse
            Collision_Impulse(new Vector3(0, 0.01f, 0), new Vector3(0, 1, 0));
            Collision_Impulse(new Vector3(2, 0, 0), new Vector3(-1, 0, 0));

            // Part III: Update position & orientation
            Vector3 x_0 = transform.position;
            Quaternion q_0 = transform.rotation;
            //Update linear status
            Vector3 x = x_0 + dt * v;
            //Update angular status
            Vector3 dw = 0.5f * dt * w;
            Quaternion qw = new Quaternion(dw.x, dw.y, dw.z, 0.0f);
            Quaternion q = Add(q_0, qw * q_0);

            // Part IV: Assign to the object
            transform.position = x;
            transform.rotation = q;
        }
    }

    bool launched = false;
    float dt = 0.015f;
    Vector3 v = new Vector3(0, 0, 0);   // velocity
    Vector3 w = new Vector3(0, 0, 0);   // angular velocity

    float mass;                                 // mass
    Matrix4x4 I_ref;                            // reference inertia

    float linear_decay = 0.999f;                // for velocity decay
    float angular_decay = 0.98f;
    float restitution = 0.5f;                   // for collision
    float friction = 0.2f;					// 摩擦系数

    private Vector3 gravity = new Vector3(0, -9.8f, 0f);
}
