namespace LearnPhysics
{
    public static class IdFactory
    {
        /// <summary>
        /// 获取Id
        /// </summary>
        /// <returns></returns>
        public static ulong AcquireId()
        {
            return m_globalIdIndex++;
        }


        /// <summary>
        /// 全局Id索引
        /// </summary>
        private static ulong m_globalIdIndex;
    }
}
