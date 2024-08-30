Editor Toolbar Extender 使用说明
---
![1.png](src/1.png)
## 说明

### 工具用于扩展 Unity Editor 的工具栏 , 方便添加自定义的按钮或功能. 使用属性标签可以快速实现

## 参数说明

| 序号 | 参数名      | 说明                                                                                                                                                                                                                                                                                  |
|----|----------|-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| 1  | Category | 按钮名称                                                                                                                                                                                                                                                                                |
| 2  | Position | GUI要绘制的位置,EditorToolbarAttribute.Anchor中的4个枚举值对应图中的4个区域<br/>1.工具栏左侧区域 - EditorToolbarAttribute.Anchor.Left<br/>2.播放按钮左侧区域 - EditorToolbarAttribute.Anchor.CenterLeft<br/>3.播放按钮右侧区域 - EditorToolbarAttribute.Anchor.CenterRight<br/>4.工具栏右侧区域 - EditorToolbarAttribute.Anchor.Right |
| 3  | Path     | 如果是下拉菜单,填入菜单路径,如果只是按钮,无需填写                                                                                                                                                                                                                                                          |
| 5  | Custom   | 如果传入True,则为自定义GUI方法                                                                                                                                                                                                                                                                 |
| 6  | Order    | 排序                                                                                                                                                                                                                                                                                  |

## 示例

```csharp
// 自定义 GUI
// 设置位置, 并标记 custom 为 true, 其他字段默认
[ EditorToolbarMenu(anchor : EditorToolbarMenuAttribute.Anchor.Left , custom : true , order : 10) ]
public static void OnToolbarGUI()
{
    value = GUILayout.HorizontalSlider(value , 0 , 10 , GUILayout.Width(100));
}

// 下拉菜单
// 设置位置, 设置 category 即下拉菜单按钮显示的文字, 并且设置 path 即实际响应按钮在下拉菜单中的层级 
[ EditorToolbarMenu(anchor : EditorToolbarMenuAttribute.Anchor.Right , category : "工具" , path : "PrefabPrefs 浏览器") ]
public static void ShowExample()
{
    var window = GetWindow<PlayerPrefsBrowser>();
    window.titleContent = new("PrefabPrefs 浏览器");
}

// 按钮
// 设置位置, 设置 category 即按钮显示的文字 
[EditorToolbarMenu(anchor : EditorToolbarMenuAttribute.Anchor.CenterRight , category : "Editor Icons 浏览器") ]
public static void ShowExample2()
{
    var window = CreateWindow<EditorIconsBrowser>("Editor Icons 浏览器");
    window.ShowUtility();
    window.minSize = new(550 , 450);
}
```

