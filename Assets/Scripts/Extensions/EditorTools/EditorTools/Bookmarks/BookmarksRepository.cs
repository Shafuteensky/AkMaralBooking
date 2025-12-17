using System.Collections.Generic;
using Extensions.Data;

namespace Extensions.EditorTools.Bookmarks
{
    /// <summary>
    /// Сохранение и загрузка закладок ObjectBookmarksWindow
    /// </summary>
    public static class BookmarksRepository
    {
        private const string FILE_NAME = "ObjectBookmarks";

        private static BookmarksFile _cache;
        private static bool _loaded;

        public static IReadOnlyList<BookmarkData> Items
        {
            get
            {
                EnsureLoaded();
                return _cache.Items;
            }
        }

        public static void Add(BookmarkData data)
        {
            EnsureLoaded();
            _cache.Items.Add(data);
            Save();
        }

        public static void Remove(BookmarkData data)
        {
            EnsureLoaded();
            _cache.Items.Remove(data);
            Save();
        }

        public static void RemoveAt(int index)
        {
            EnsureLoaded();
            if (index >= 0 && index < _cache.Items.Count)
            {
                _cache.Items.RemoveAt(index);
                Save();
            }
        }

        public static void Clear()
        {
            EnsureLoaded();
            _cache.Items.Clear();
            Save();
        }

        private static void EnsureLoaded()
        {
            if (_loaded)
            {
                return;
            }

            _cache = SaveLoadManager.Load(FILE_NAME, new BookmarksFile()) ?? new BookmarksFile();
            _loaded = true;
        }

        private static void Save()
        {
            if (!_loaded)
            {
                return;
            }

            SaveLoadManager.Save(_cache, FILE_NAME);
        }
    }
}