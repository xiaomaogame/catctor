# 前言

作者：小猫学游戏  （同B站名称）

该项目使用前后端分离开发，目的是实现分层动画资源的装配和管理，由此生成自定义组合的2D动画人物，并支持导出和编辑等功能。

前端技术栈：JS 、 Vue2.0 、   Canvas 、 Zrender  、ElmentUI

后端技术栈：C#  、dotNet8、 WebAPI、SqlSugar

数据库：Sqlite



 # 运行介绍
 目前仅提供了带环境的win64 发布版，后续会提供更多环境

下载发布的Release 包，解压后 找到 CharacterAPI.exe

![image](https://github.com/xiaomaogame/catctor/assets/147690046/628c7a08-dae7-4c40-9f31-ec96a70ec50d)

双击后运行，然后打开浏览器输入 localhost:21422

![image](https://github.com/xiaomaogame/catctor/assets/147690046/e25f6579-a1bb-4392-a605-114f323dd227)

![image](https://github.com/xiaomaogame/catctor/assets/147690046/d7836bce-a27a-44f3-b12f-c3715c1da3e7)

# 使用介绍

目前提供的功能有

主页：装配区

装配区是我们的主要组装和预览区域，左侧是装配资源，中间是预览效果，右侧是导出功能和证书信息

在主页的装配区可以快速修改素材信息，只需要在图标上右键
![image](https://github.com/xiaomaogame/catctor/assets/147690046/8d704ff1-3274-4faf-a13a-1e10cbbecace)

上传文件：自定义上传素材
上传文件提供了丰富的功能，可以自定义资源的各项指标
![image](https://github.com/xiaomaogame/catctor/assets/147690046/76b18429-876d-4a9b-87d4-435703816b7e)


层级管理：自定义层级数据
层级管理可以定义60个层级，从-30到30
![image](https://github.com/xiaomaogame/catctor/assets/147690046/74067748-e8bc-47a7-bb7c-7503b95fb3ca)

# 编译介绍
由于项目的复杂性，这里需要比较专业的技术栈知识

分别是前端Vue的编译和后端WebAPI的编译，这里不做具体说明



