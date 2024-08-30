Preview Editor 使用说明
---
![1.png](src/1.png)
## 说明

### 用于进行模型预览, 支持预览画面的移动/缩放/旋转, 通过扩展 Module 支持动画播放/显示骨骼等一系列自定义功能, 可以将此 Editor 嵌套入自己的 CustomEditorWindow 中.

## 使用方法

### 1. 在 EditorWindow 中定义 AvatarEditor 对象, 添加需要的 Module.
```csharp
private void OnEnable()
{
    _avatarEditor = new(this);
    
    // 添加动画播放模块
    _avatarEditor.AddModule<PreviewAnimationModule>();
    // 添加骨骼列表模块
    _avatarEditor.AddModule<ModelBonesModule>();
}
```

### 2. 分别在 EditorWindow 的 Update 方法和 OnGUI 方法调用 AvatarEditor 的 Update 方法和 Draw 方法.
```csharp
private void Update()
{
    _avatarEditor?.Update();
}

private void OnGUI()
{
    _avatarEditor.Draw(position);
}
```

### 3. 在EditorWindow 销毁时, 调用 AvatarEditor 的 Dispose 方法进行释放.
```csharp
private void OnDestroy()
{
    _avatarEditor.Dispose();
}
```
***可以参考 [PreviewEditorWindow.cs](../PreviewEditorWindow.cs)***

### 4. 如果需要添加自己的 Module , 只要自己的 Module 继承 IPreviewModule 接口, 调用 AvatarEditor.AddModule 进行添加即可.

***可以参考 [PreviewAnimationModule.cs](../Module/PreviewAnimationModule.cs)***