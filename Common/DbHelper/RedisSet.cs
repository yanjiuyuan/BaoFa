using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DbHelper
{
    public class RedisSet : RedisBase
    {
        #region 添加
        /// <summary>
        /// key集合中添加value值
        /// </summary>
        public static void Add(string key, string value)
        {
            RedisBase.Core.AddItemToSet(key, value);
        }
        /// <summary>
        /// key集合中添加list集合
        /// </summary>
        public static void Add(string key, List<string> list)
        {
            RedisBase.Core.AddRangeToSet(key, list);
        }
        #endregion
        #region 获取
        /// <summary>
        /// 随机获取key集合中的一个值
        /// </summary>
        public static string GetRandomItemFromSet(string key)
        {
            return RedisBase.Core.GetRandomItemFromSet(key);
        }
        /// <summary>
        /// 获取key集合值的数量
        /// </summary>
        public static long GetCount(string key)
        {
            return RedisBase.Core.GetSetCount(key);
        }
        /// <summary>
        /// 获取所有key集合的值
        /// </summary>
        public static HashSet<string> GetAllItemsFromSet(string key)
        {
            return RedisBase.Core.GetAllItemsFromSet(key);
        }
        #endregion
        #region 删除
        /// <summary>
        /// 随机删除key集合中的一个值
        /// </summary>
        public static string PopItemFromSet(string key)
        {
            return RedisBase.Core.PopItemFromSet(key);
        }
        /// <summary>
        /// 删除key集合中的value
        /// </summary>
        public static void RemoveItemFromSet(string key, string value)
        {
            RedisBase.Core.RemoveItemFromSet(key, value);
        }
        #endregion
        #region 其它
        /// <summary>
        /// 从fromkey集合中移除值为value的值，并把value添加到tokey集合中
        /// </summary>
        public static void MoveBetweenSets(string fromkey, string tokey, string value)
        {
            RedisBase.Core.MoveBetweenSets(fromkey, tokey, value);
        }
        /// <summary>
        /// 返回keys多个集合中的并集，返还hashset
        /// </summary>
        public static HashSet<string> GetUnionFromSets(string[] keys)
        {
            return RedisBase.Core.GetUnionFromSets(keys);
        }
        /// <summary>
        /// keys多个集合中的并集，放入newkey集合中
        /// </summary>
        public static void StoreUnionFromSets(string newkey, string[] keys)
        {
            RedisBase.Core.StoreUnionFromSets(newkey, keys);
        }
        /// <summary>
        /// 把fromkey集合中的数据与keys集合中的数据对比，fromkey集合中不存在keys集合中，则把这些不存在的数据放入newkey集合中
        /// </summary>
        public static void StoreDifferencesFromSet(string newkey, string fromkey, string[] keys)
        {
            RedisBase.Core.StoreDifferencesFromSet(newkey, fromkey, keys);
        }
        #endregion

        #region -- SortedSet --
        /// <summary>
        ///  添加数据到 SortedSet
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="t"></param>
        /// <param name="score"></param>
        public static bool SortedSet_Add<T>(string key, T t, double score)
        {

            string value = ServiceStack.Text.JsonSerializer.SerializeToString<T>(t);
            return RedisBase.Core.AddItemToSortedSet(key, value, score);

        }
        /// <summary>
        /// 移除数据从SortedSet
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool SortedSet_Remove<T>(string key, T t)
        {

            string value = ServiceStack.Text.JsonSerializer.SerializeToString<T>(t);
            return RedisBase.Core.RemoveItemFromSortedSet(key, value);

        }
        /// <summary>
        /// 修剪SortedSet
        /// </summary>
        /// <param name="key"></param>
        /// <param name="size">保留的条数</param>
        /// <returns></returns>
        public static long SortedSet_Trim(string key, int size)
        {

            return RedisBase.Core.RemoveRangeFromSortedSet(key, size, 9999999);

        }
        /// <summary>
        /// 获取SortedSet的长度
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static long SortedSet_Count(string key)
        {

            return RedisBase.Core.GetSortedSetCount(key);

        }

        /// <summary>
        /// 获取SortedSet的分页数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static List<T> SortedSet_GetList<T>(string key, int pageIndex, int pageSize)
        {

            var list = RedisBase.Core.GetRangeFromSortedSet(key, (pageIndex - 1) * pageSize, pageIndex * pageSize - 1);
            if (list != null && list.Count > 0)
            {
                List<T> result = new List<T>();
                foreach (var item in list)
                {
                    var data = ServiceStack.Text.JsonSerializer.DeserializeFromString<T>(item);
                    result.Add(data);
                }
                return result;
            }

            return null;
        }


        /// <summary>
        /// 获取SortedSet的全部数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static List<T> SortedSet_GetListALL<T>(string key)
        {
            var list = RedisBase.Core.GetRangeFromSortedSet(key, 0, 9999999);
            if (list != null && list.Count > 0)
            {
                List<T> result = new List<T>();
                foreach (var item in list)
                {
                    var data = ServiceStack.Text.JsonSerializer.DeserializeFromString<T>(item);
                    result.Add(data);
                }
                return result;
            }

            return null;
        }

        /// <summary>
        /// 设置缓存过期
        /// </summary>
        /// <param name="key"></param>
        /// <param name="datetime"></param>
        public static void SortedSet_SetExpire(string key, DateTime datetime)
        {

            RedisBase.Core.ExpireEntryAt(key, datetime);

        }
        #endregion
    }
}