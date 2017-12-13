using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Storage {
    [Serializable]
    class DictionaryStore<TKey, TValue> : Dictionary<TKey, TValue> {
        private String path;

        public DictionaryStore(String path) {
            this.path = path;
            this.load();
        }

        public void load() {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream f;
            try {
                f = new FileStream(this.path, FileMode.Open);
            }
            catch (IOException e) { return; } //it's not a problem if there's nothing to load
            Dictionary<TKey, TValue> d;
            try {
                d = (Dictionary<TKey, TValue>)formatter.Deserialize(f);
            }
            catch (SerializationException e) { return; } //it's not the end of the world if we fail to load
            finally {
                f.Close();
            }
            foreach (TKey key in d.Keys) {
                this[key] = d[key];
            }
        }

        public void save() {
            Dictionary<TKey, TValue> d = new Dictionary<TKey, TValue>();
            foreach (TKey key in this.Keys) {
                d[key] = this[key];
            }
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream f = new FileStream(this.path, FileMode.Create);
            formatter.Serialize(f, d);
            f.Close();
        }
    }
}
