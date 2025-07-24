<!--===================================================================
    MainLayout (tOrder.Shell)

    Primary application layout containing:
    ┌────────────────────────────────────────────────────────────┐
    │ 1. NavigationView (left-side, minimized)                   │
    │ 2. TopBar (header section, scaled via Viewbox)             │
    │ 3. ContentFrame (dynamic page content)                     │
    └────────────────────────────────────────────────────────────┘
    All visual parts are wrapped in scalable containers.
====================================================================-->

<UserControl
    x:Class="tOrder.Shell.MainLayout"
    x:Name="MainLayoutControl"

    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:local="using:tOrder.Shell"
    xmlns:common="using:tOrder.Common"
    xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
    xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"

    mc:Ignorable="d"
    d:DesignWidth="1024"
    d:DesignHeight="768"
    d:DataContext="{d:DesignInstance Type=local:MainLayoutVM, IsDesignTimeCreatable=True}"

    x:DataType="local:MainLayoutVM"

    Width="Auto"
    Height="Auto"
    MinWidth="{Binding Source={StaticResource LayoutConfigProxy}, Path=Data.MinWindowWidth}"
    MinHeight="{Binding Source={StaticResource LayoutConfigProxy}, Path=Data.MinWindowHeight}"
    Background="Transparent"
    HorizontalAlignment="Stretch"
    VerticalAlignment="Stretch"
    Opacity="1.0"
    Visibility="Visible"
    RenderTransformOrigin="0.5,0.5"

    common:DesignProperties.Control="MainLayoutControl"
    common:DesignProperties.Position="Top"
    common:DesignProperties.GroupTag="Layout"
    common:DesignProperties.GroupSubTag="Shell"
    common:DesignProperties.DesignWidth="1024"
    common:DesignProperties.DesignHeight="768">

    <!--===========================================================
        Resources: Layout binding proxy for scale and width config
    =============================================================-->
    <UserControl.Resources>
        <common:BindingProxy x:Key="LayoutConfigProxy"
                             Data="{x:Bind LayoutConfig, Mode=OneWay}" />
    </UserControl.Resources>

    <!--===========================================================
        Main Grid: Wraps NavigationView with internal layout
    =============================================================-->
    <Grid x:Name="MainLayoutRoot">

        <!--=======================================================
            🧭 NavigationView (left minimal menu)
        =========================================================-->
        <NavigationView
            x:Name="NavView"
            PaneDisplayMode="LeftMinimal"
            IsBackButtonVisible="Collapsed"
            IsSettingsVisible="False"
            IsPaneOpen="False"
            IsPaneToggleButtonVisible="False"
            IsTabStop="True"
            SelectionChanged="NavigationView_SelectionChanged">

            <!-- 📋 Main navigation menu items -->
            <NavigationView.MenuItems>
                <NavigationViewItem IsSelected="True"
                                    Margin="0,12,0,0"
                                    Icon="Setting"
                                    Content="Übersicht per IPC"
                                    Tag="OverviewByIPC" />
                <NavigationViewItem Content="CapacityUnitDashboard"
                                    Tag="CapacityUnitDashboard" />
                <NavigationViewItem Icon="Contact"
                                    Content="Übersicht per Betreiber"
                                    Tag="OverviewByBetreiber" />
                <NavigationViewItem Icon="Switch"
                                    Content="Schicht Anfang"
                                    Tag="SchichtAnfang" />
                <NavigationViewItem Icon="Cancel"
                                    Content="Schicht Ende"
                                    Tag="SchichtEnde" />
                <NavigationViewItem Icon="Repair"
                                    Content="Rüsten"
                                    Tag="Rusten" />
            </NavigationView.MenuItems>

            <!--=======================================================
                📦 Main content area – TopBar and dynamic Page content
            =========================================================-->
            <Grid Background="{StaticResource BackgroundColor}">
                <Grid.RowDefinitions>
                    <RowDefinition Height="Auto" />
                    <!-- TopBar -->
                    <RowDefinition Height="*"    />
                    <!-- Page Content -->
                </Grid.RowDefinitions>

                <!-- 🔝 TopBar section (scalable via Viewbox) -->
                <Viewbox x:Name="TopBarViewbox"
                         Stretch="Uniform"
                         Height="Auto">
                    <Grid x:Name="TopBarContainer"
                          Width="{Binding Source={StaticResource LayoutConfigProxy}, Path=Data.DesignWidth}">
                        <Grid.RenderTransform>
                            <ScaleTransform
                                ScaleX="{Binding Source={StaticResource LayoutConfigProxy}, Path=Data.LayoutScale}"
                                ScaleY="{Binding Source={StaticResource LayoutConfigProxy}, Path=Data.LayoutScale}" />
                        </Grid.RenderTransform>
                        <local:TopBar />
                    </Grid>
                </Viewbox>

                <!-- 📄 Dynamic page content (navigated Frame) -->
                <Frame x:Name="ContentFrame"
                       Grid.Row="1"
                       HorizontalAlignment="Stretch"
                       VerticalAlignment="Stretch" />
            </Grid>
        </NavigationView>
    </Grid>
</UserControl>


<!--
=====================================================================
📐 MainLayout.xaml – Visual Structure Overview
=====================================================================

This file defines the root layout structure of the tOrder application.
It hosts the navigation shell and page content area and scales its top
bar according to centralized layout configuration.

🧩 Root Structure:
────────────────────────────────────────────
UserControl (MainLayout)
└── Grid (MainLayoutRoot)
    └── NavigationView (NavView)
        └── Grid (content layout)
            ├── Viewbox (TopBarViewbox)
            │   └── Grid (TopBarContainer)
            │       └── TopBar (UserControl)
            └── Frame (ContentFrame)

────────────────────────────────────────────

📘 UI Element Breakdown:
────────────────────────────────────────────

1️⃣ Grid: MainLayoutRoot
    - Root layout container for the entire shell.
    - Hosts the NavigationView (left menu and content).

2️⃣ NavigationView: NavView
    - Left-hand menu (collapsed to minimal by default).
    - MenuItems include major app navigation routes.
    - Collapsible panel with icons and labels.

3️⃣ Grid (inside NavView)
    - Defines the layout of the visual area with 2 rows:
        Row 0 → TopBarViewbox
        Row 1 → ContentFrame

4️⃣ Viewbox: TopBarViewbox
    - Wraps and scales the top bar according to layout scale.
    - Uses Stretch="Uniform" for proportional scaling.

5️⃣ Grid: TopBarContainer
    - Fixed-width container for TopBar UserControl.
    - Width is bound to LayoutConfig.DesignWidth.
    - RenderTransform.ScaleTransform is bound to LayoutConfig.LayoutScale.

6️⃣ TopBar (UserControl)
    - Displays heading, breadcrumbs, user session and actions.

7️⃣ Frame: ContentFrame
    - The main content area.
    - Displays pages based on navigation selection.
    - Stretch-aligned to always fill remaining space.

────────────────────────────────────────────

🧩 Data Binding:
────────────────────────────────────────────

- 🔄 `LayoutConfigProxy` is a `BindingProxy` resource providing access
  to the `LayoutConfigVM` instance.

- `TopBarContainer.Width` is bound to:
      → `LayoutConfig.DesignWidth`

- `TopBarContainer.ScaleTransform` is bound to:
      → `LayoutConfig.LayoutScale`

This allows global layout configuration to control sizing and scaling
of the TopBar and any future components added similarly.

────────────────────────────────────────────

✅ Summary
────────────────────────────────────────────

This layout is fully modular and adaptive, utilizing centralized layout
configuration (`LayoutConfigVM`) for width and scale. It separates shell
logic (navigation and top bar) from content and allows per-page content
to be loaded dynamically via `ContentFrame`.

Designed for production-grade scaling and responsive layout behavior.
-->




# ✅ Design Principles for XAML Layouts in tOrder (Style: MainLayout)

A structured guide for maintaining consistent, scalable, and readable XAML files across the `tOrder` project.

---

## 1. 🌍 ASCII Header Comment Block

Each `.xaml` file must start with a descriptive, structured comment:

```xml
<!--===================================================================
    ComponentName (Namespace)

    High-level purpose / role of this component
    └── Visual breakdown:
    ┌─────────────────────────────────────┐
    │ 1. Element purpose / placement              │
    │ 2. Element purpose / placement              │
    │ 3. Optional layout/scale behavior           │
    └─────────────────────────────────────┘
====================================================================-->
```

---

## 2. 🔹 UserControl Declaration with Grouped Attributes

Organize `UserControl` attributes by logical groups:

### 🔸 Metadata & Context

```xml
x:Class="..."
x:Name="..."
x:DataType="..." (required for x:Bind)
```

### 🔸 Namespaces & Design-time

```xml
xmlns:common="using:tOrder.Common"
mc:Ignorable="d"
d:DesignWidth="..."
d:DataContext="{d:DesignInstance ...}"
```

### 🔸 Layout Attributes

```xml
Width="Auto"
Height="Auto"
MinWidth="{Binding ...}"
HorizontalAlignment="Stretch"
```

### 🔸 Project Metadata (DesignProperties)

```xml
common:DesignProperties.Control="..."
common:DesignProperties.Page="..."
common:DesignProperties.Position="..."
```

---

## 3. 🌐 Layout Regions via Comment Blocks

Structure visual content using `<!--=========================================================== -->` blocks.

```xml
<!--===========================================================
    Resources: Layout binding proxy for scale and width config
============================================================-->
```

---

## 4. 🔗 Use BindingProxy for Layout Configuration

In `<UserControl.Resources>`, define layout access:

```xml
<common:BindingProxy x:Key="LayoutConfigProxy"
                     Data="{x:Bind LayoutConfig, Mode=OneWay}" />
```

Used for:

* Width / Height binding
* ScaleTransform
* Min/max dimensions

---

## 5. 🌿 Use Viewbox for Scalable Sections

Wrap headers like `TopBar` in a `Viewbox`:

* Inner `Grid` has width bound to `LayoutConfig.DesignWidth`
* `ScaleTransform` is bound to `LayoutConfig.LayoutScale`

---

## 6. 🌐 Identify Layout Blocks with `DesignProperties`

Use attached properties:

```xml
common:DesignProperties.Control="MainLayout"
common:DesignProperties.Page="Shell"
common:DesignProperties.GroupTag="Layout"
```

Used for visual tooling, runtime inspection, and automated testing.

---

## 7. 🔹 Visual Summary Comment at End of File

Add a detailed visual description:

```xml
<!--
=====================================================================
📊 Component.xaml – Visual Structure Overview
=====================================================================

📈 Root Layout Structure
...

📜 UI Element Breakdown
...

🔗 Data Binding
...

✅ Summary
...
-->
```

---

## 🛠 Recommended .xaml Sections Order

1. 📄 ASCII Header
2. 🌟 UserControl declaration with grouped attributes
3. 🔹 Resources + BindingProxy
4. 📍 Root container (Grid/Panel)
5. 📊 TopBar/Header layout section
6. 📜 Main content section (Frame, TabView)
7. 🔹 Closing comment with visual breakdown

---

## ✅ Why This Structure?

| Goal                        | Benefit                                  |
| --------------------------- | ---------------------------------------- |
| Consistent visual style     | Easier orientation across components     |
| Self-descriptive XAML       | Understand purpose without code digging  |
| Strong `x:Bind` + Proxy use | High-performance + flexible layout logic |
| `DesignProperties` metadata | Metadata-driven design & debugging       |
| Testability & tooling ready | For automated UI inspectors and overlays |
