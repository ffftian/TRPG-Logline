# TRPG-LOGline

TRPG-LOGline是一款革命性的Unity插件，用于骨骼动画跑团视频的制作演出。
spine运行库版本为4.1，所操作的版本为Unity 2021.3.6f1。  
本教程适用于对Unity基础操作有了解的人使用。

# 目录
- [TRPG-LOGline](#trpg-logline)
- [目录](#目录)
  - [工程配置](#工程配置)
- [编辑器操作](#编辑器操作)
- [时间线相关](#时间线相关)
- [动画相关](#动画相关)
  - [spine口型同步](#口型同步)
- [代码相关](#代码相关)
  - [添加扩展轨道脚本](#添加扩展轨道脚本)


# 工程配置
未提及目录则无需操作上的注意。  
AssetDialogue = 存放原始log的位置。  
AssetSpeak = 存放每名角色音频的位置。  
AssetSpine = 存放每名角色骨骼动画实体的位置。
PipeLine = 渲染管线相关，如不了解勿动。  
Prefab = 预制体位置。  
Presentation Material = 教程时使用的工程文件（可删除）  
Resources/QQTimeLine = 自动生成的轨道动画读取位置，不建议从文件夹预览  
Scenes = 场景摆放位置
ScriptExtension = 额外轨道生成脚本扩展，扩展轨道生成参考[添加扩展轨道脚本](##添加扩展轨道脚本)  
ShowScript = 如有编程需要，建议将您写的脚本放置于此。  
Sources = 资源目录。  
TRPGEditorSetting = 本插件的配置文件。  


# 编辑器操作


# 时间线(Timeline)相关


# spine动画相关
## 默认
默认动画的名称需命名为idle，当创建轨道时会自动等同于音频时间的创建idle动画，并循环播放。
## 口型
在spine Anmation中新建一个文件夹Mouth。
所有张嘴口型动画以Mouth_MouthO[x]命名
所有闭嘴口型以Mouth_MouthC[x]命名
(x代表0-9)
在动画中只需要为调整自己的嘴型附件。
声音越响时，调用的张嘴口型数值越大。
闭嘴口型则用于衔接同名的张嘴口型，在声音低于阈值时调用。
一般建议制作序号1到序号5种的MouthO/MouthC既足够表现说话了。

# 代码相关
以下内容默认您对在Unity中用C#编程具备一定了解。
## 添加扩展轨道脚本
轨道扩展示例位于 Assets/ScriptExtension
在使用IDE时，会分为两个IDE
Assembly-CSharp中的TimelineSelectExtension
TimelineSelect用于DialogComponent在选中Timeline资源时，直接将场景中特定对象赋值给特定轨道脚本。
扩展可以在相似位置安装相同格式创建静态类并添加静态函数。
并以TimelineSelectAttribute特性包装静态函数。


Assembly-CSharp-Editor中的 Editor/TimelineCreaterExtension
QQLogTimelineOdinEditorWindow在动态创建轨道时，会进行额外轨道的赋值。

