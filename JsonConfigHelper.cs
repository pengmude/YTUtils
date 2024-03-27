// -----------------------------------------------------------------------------
// 文件名称：JsonConfigHelper.cs
// 作者：彭木德
// 版本：1.0
// 创建日期：2024-3-27
// 描述：这是一个用于读写和管理JSON配置文件的助手类，属于YTUtils.JsonConfigure命名空间。通过Json.NET库，该类能够轻松加载、编辑以及保存JSON配置数据。主要功能包括：
//   - 加载指定文件名的JSON配置文件，若文件不存在则创建一个空JSON对象并写入新文件；
//   - 向JSON配置对象中插入或更新键值对，并实时同步至文件；
//   - 根据嵌套键路径从JSON配置对象中读取特定键值；
//   - 确保整个过程中的数据一致性与正确性。
// 使用示例请参阅各方法内的注释说明。
// -----------------------------------------------------------------------------
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;

namespace YTUtils.JsonConfigure
{
    public class JsonConfigHelper
    {
        private JObject _jsonConfig = new JObject();
        private string _configFileName;

        public JsonConfigHelper() { }

        // 提供接口加载指定文件名的配置文件为Json对象，如果不存在则创建该文件并创建空Json对象
        public void LoadConfig(string configFileName)
        {
            _configFileName = configFileName;
            if (File.Exists(configFileName))
            {
                string jsonData = File.ReadAllText(configFileName);
                // 检查是否读取到有效内容
                if (string.IsNullOrEmpty(jsonData))
                {
                    jsonData = "{}";
                }
                _jsonConfig = JsonConvert.DeserializeObject<JObject>(jsonData);
            }
            else
            {
                // 获取文件所在目录路径
                string directoryPath = Path.GetDirectoryName(configFileName);

                // 如果目录为空或"."（表示当前目录），则尝试创建文件（假设在这种情况下应创建在当前目录）
                if (string.IsNullOrEmpty(directoryPath) || directoryPath == ".")
                {
                    try
                    {
                        File.WriteAllText(configFileName, "{}");
                        _jsonConfig = JsonConvert.DeserializeObject<JObject>("{}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"无法创建配置文件 {_configFileName}：{ex.Message}");
                    }
                }
                else
                {
                    // 如果目录非空且实际不存在，则弹窗提示文件不存在
                    if (!Directory.Exists(directoryPath))
                    {
                        Console.WriteLine($"配置文件 {_configFileName} 所在的目录 {directoryPath} 不存在！");
                    }
                    else
                    {
                        // 这里理论上不会执行到，因为File.Exists已经判断过文件不存在
                        // 可能是其他原因导致文件读取失败，可以添加相应处理逻辑或保持原有异常处理
                    }
                }
            }
        }

        /// <summary>
        /// 提供插入配置到Json对象的方法，键值不存在则新建，插入操作同时会写入到文件中
        /// </summary>
        /// <param name="value"></param>
        /// <param name="keys"></param>
        #region 双击此处查看函数使用示例
        /* 调用形如：
         * JsonConfigHelper helper = new JsonConfigHelper("config.json");
         * helper.WriteValue("NewProjectValue", "新建项目配置", "配置1");
         * helper.WriteValue("NewSolutionValue", "打开方案配置", "配置2");
         * helper.WriteValue("DeeplyNestedValue", "深层配置", "一级嵌套", "二级嵌套", "键值");
         * 
         * 保存配置形如：
        "新建项目配置": 
        {
            "配置1": "NewProjectValue"
        },
        "打开方案配置": 
        {
            "配置2": "NewSolutionValue"
        },
        "深层配置": 
        {
            "一级嵌套": 
            {
                "二级嵌套": 
                {
                    "键值": "DeeplyNestedValue"
                }
            }
        }
        */
        #endregion
        public void WriteValue(string value, params string[] keys)
        {
            JObject currentJson = _jsonConfig;
            for (int i = 0; i < keys.Length; i++)
            {
                if (i < keys.Length - 1)
                {
                    // 如果不是最后一层键，则继续深入到下一层 JSON 对象
                    if (!currentJson.TryGetValue(keys[i], out JToken nextValue) || !(nextValue is JObject childJson))
                    {
                        // 如果当前层级没有找到对应的键或其值不是 JObject，则创建一个新的 JObject
                        childJson = new JObject();
                        currentJson[keys[i]] = childJson;
                    }
                    currentJson = childJson;
                }
                else
                {
                    // 如果是最后一层键，则设置其值
                    currentJson[keys[i]] = JToken.FromObject(value);
                }
            }
            WriteToFile();
        }
        /// <summary>
        /// 读取 JSON 对象中嵌套键名的键值的方法
        /// </summary>
        /// <param name="keys">可以填多个层级的key名</param>
        /// <returns></returns>
        #region 双击此处查看函数使用示例
        /*配置形如：
        {
          "父节点0": 
          {
            "子节点0": "12345678",
	        "子节点1":
	        {
		        "孙节点0":"12345678"
	        }
          },
          "父节点1": 
          {
            "子节点0": "12345678"
          }
        }
        JsonConfigHelper config = new JsonConfigHelper("config.json");
        string value = (string)config.ReadValue("父节点0", "子节点0"); // 此处返回 "12345678"
        string value = (string)config.ReadValue("父节点0", "子节点1", "孙节点0"); // 此处返回 "123"
        string value = (string)config.ReadValue("父节点0", "子节点2"); // 此处返回 "",因为不存在第二个参数的key
        */
        #endregion
        public string ReadValue(params string[] keys)
        {
            JObject currentJson = _jsonConfig;
            JToken currentValue = null;
            for (int i = 0; i < keys.Length; i++)
            {
                if (currentJson == null)
                {
                    break;
                }

                if (currentJson.TryGetValue(keys[i], out JToken nextValue))
                {
                    currentValue = nextValue;
                    if (i < keys.Length - 1 && nextValue.Type == JTokenType.Object)
                    {
                        currentJson = nextValue as JObject;
                    }
                    else
                    {
                        // 如果已经是最后一层或遇到非对象类型的值，则返回当前值转换为 string 类型
                        break;
                    }
                }
                else
                {
                    return null;
                }
            }

            return currentValue?.ToString();
        }

        private void WriteToFile()
        {
            string jsonData = JsonConvert.SerializeObject(_jsonConfig, Formatting.Indented);
            File.WriteAllText(_configFileName, jsonData);
        }
    }
}