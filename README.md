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

![image](https://github.com/xiaomaogame/catctor/assets/147690046/56558449-c9fb-4e5d-a24a-5a82931dba4d)

# 使用介绍

目前提供的功能有

## 主页：装配区

装配区是我们的主要组装和预览区域，左侧是装配资源，中间是预览效果，右侧是导出功能和证书信息

在主页的装配区可以快速修改素材信息，只需要在图标上右键
![image](https://github.com/xiaomaogame/catctor/assets/147690046/e43cddcd-8ecc-4624-96ff-0de0fef19fc0)


## 上传文件：自定义上传素材
上传文件提供了丰富的功能，可以自定义资源的各项指标

单击图片素材可以选择当前素材的Icon 图标

男女选项将会根据 文件名称自动解析 

文件名称默认提供一个全局唯一的，不会产生冲突

![image](https://github.com/xiaomaogame/catctor/assets/147690046/a8a79476-e59b-4d7b-a26b-0ea0b7526f5e)

## 关于上传中的多图层选项

有时我们为了节省制作成本，会将两个素材分为前后图层

以尾巴素材为例：
当尾巴出现在人物后方时，如果采用单一图层的方式，我们需要进行抠图处理，并且可能无法正确的完成遮罩
如果将素材分为前后两个图层，那么直接省去了抠图的麻烦

图层有两个选项：前  后
前图层为主图层，将会正常展示在主页，后图层不会进行展示

关于前后图层的渲染：

仍然以尾巴为例，假设尾巴的前图层在5，那么后图层的渲染就在 -5

前后图层的关联：

当你分别上传了 一个素材的前后图层后，我们切换到 【项目管理】
找到前图层，并配置 AfterId 为后图层的ID
配置完成后，我们在主页上将会得到一个带有组合标记的图标

![image](https://github.com/xiaomaogame/catctor/assets/147690046/53d7aa6e-40d7-40f7-9fd4-2090067511f3)



## 层级管理：自定义层级数据
层级管理可以定义60个层级，从-30到30
请注意多图层渲染会占用负图层，请尽量不使用负图层，以免产生冲突
如果一定要使用，请使用在正图层没有出现的负数

![image](https://github.com/xiaomaogame/catctor/assets/147690046/1a74a163-55a3-42cc-945c-a03f23f023ae)


## 项目管理
除了首页的便捷管理，这里提供了更多的项目管理功能

请注意多图层的关联使用方式：

如果你的图层设置为前图层，那么将会出现关联 AfterId选项，只需要填入对应的 后图层Id即可打成一组

![image](https://github.com/xiaomaogame/catctor/assets/147690046/bda89f50-0567-4206-ab42-07557fc7144d)



# 编译介绍
由于项目的复杂性，这里需要比较专业的技术栈知识

分别是前端Vue的编译和后端WebAPI的编译，这里不做具体说明



