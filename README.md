# game-upgrade · 《重生2：我独自逆袭》逆向工程与架构解析

> 对一款 **Unity 2021.3.45f2c1 (IL2CPP) + HybridCLR 热更 + Addressables + AVProVideo + CEF 内嵌浏览器** 的 **FMV 互动叙事手游** 的完整逆向分析，用于**学习游戏工程架构 / FMV 互动叙事 / 逆向方法论**。**仅供学习研究，请勿抄袭、商用或侵权。**

`git@github:tutan123/game-upgrade.git`

---

## 这是什么游戏

**"视频是骨架，玩法是挂件，数值是胶水，数据是血脉。"** 一款真人/实拍 CG 互动叙事手游：一条条剧情视频串起主线，在视频的特定时间点"打点"触发玩法（选择、QTE、战斗、酒桌、商店、小游戏、360° 视频），配一张庞大的数值网（4 维基础属性 + **18 个女主好感度** + 酒量 + 战斗 + KPI），用 **Excel 配表 + XNode 节点图** 驱动内容，用 **HybridCLR + Addressables** 支持热更运营。

## 仓库内容

| 目录 | 内容 | 说明 |
|------|------|------|
| `深度解析/` | ★ **13 章 + 索引的《深度解析合集》**（含 **10 张 mermaid 架构/流程图**）| 全部机制的深入分析，带真实源码文件名+行号 |
| `源码/Decompiled_CSharp/` | ★ **对 `Assembly-CSharp.dll` 等 18 个程序集的反编译 C#**（约 1798 个 `.cs`，含 `.csproj`）| 原始类名/方法/字段、近乎原 C# |
| `源码/*.md` | 项目分析报告 + 游戏机制与架构深度解析 | 报告 |
| `元数据/dump.cs` | **Il2CppDumper 生成的干净类名 + 内存地址**（15MB）| 做 Mod / 排查用 |
| `元数据/stringliteral.json` | **全游戏 10,347 条字符串（含中文对白）** | 剧情/文案研究 |

## 关键发现速览

- **引擎**：Unity 2021.3.45f2c1（中国版）+ IL2CPP + Burst
- **C# 热更**：HybridCLR，运行时加载 `HotUpdate.dll`
- **资源**：Addressables（531 bundle / 1.24GB）+ 混淆外置媒体（997 个伪装成 `.PNG` 的 MP4 + 710 个伪装成 `.JPG` 的 MP3，共 28GB）
- **资源混淆**：路径段 `MD5→8字节long→Base62`，所以 `hash("Video")=i55N8rvfL6a`、`hash("Nor")=djWu27ED936`
- **玩法**：FMV 互动叙事（XNode 图）+ 视频选择/QTE/战斗(如来神掌/咏春/组合拳…)/酒桌/刮刮乐/黄金矿工式抓娃/H5 弹珠台
- **数值**：77 个属性枚举，含 18 个女主好感线
- **构建失误**：`_BackUpThisFolder_ButDontShipItWithYourGame` 未删，导致源码/元数据全泄漏——这也是本仓库的由来

## 逆向方法（详见 `深度解析/03_逆向方法论.md`）

| 目标 | 工具 |
|------|------|
| `.dll` → C# | ILSpy / ilspycmd / dnSpy |
| IL2CPP → `dump.cs` + 全部文本 | Il2CppDumper（`GameAssembly.dll` + `global-metadata.dat`）|
| bundle 贴图/资源 | UnityPy（读 `Texture2D/Sprite`；**别 `read()` MonoBehaviour，会段错误**）|
| 媒体魔数识别 | 读文件头：`ftyp`=MP4、`ID3`/`FFEx`=MP3 |

## ⚠️ 未入库的大文件（保留在本机工作区）

受 GitHub 单文件 >100MB 拒绝、repo 过大被限流/封禁限制，以下**超大原始产物未 push**（在本机 `G:\Projects\Agent\Agent Cli\` 下保留完整版）：

- `重生2我独自逆袭Demo_代码提取\il2cpp_output\`（IL2CPP 全量 C++，392MB）
- `重生2我独自逆袭Demo_代码提取\il2cpp_dump\`（`script.json`42MB、`il2cpp.h`25MB、`DummyDll\`）
- `重生2我独自逆袭Demo_代码提取\managed_dlls\`（二进制 .NET 程序集）
- `重生2我独自逆袭Demo_代码提取\il2cpp_metadata\global-metadata.dat`（IL2CPP 元数据，7.9MB）
- `重生2我独自逆袭Demo_资源解包\`（从 531 bundle 解出的 ~8969 张图片/UI，约 1GB）
- `重生2我独自逆袭Demo_媒体资源\`（还原的 997 视频 + 710 音频，28GB）

> 若确需入库以上大文件，请用 **Git LFS** 并自行配置 LFS 存储；我可以按你的要求调整并重新 push。

---

*本仓库由对真实构建产物的代码级反编译分析生成，非二手资料。仅供学习研究。*
