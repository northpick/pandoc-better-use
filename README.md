# Pandoc Better Use

一个现代化的 WPF 图形界面工具，让 Pandoc 文档转换更简单、更强大！

![Pandoc Better Use 界面](https://img.shields.io/badge/UI-WPF_Modern-blue)
![.NET 8](https://img.shields.io/badge/.NET-8-purple)
![Pandoc 支持](https://img.shields.io/badge/Pandoc-所有格式-green)

## ✨ 特性亮点

### 🎨 **现代化界面**
- 基于 WPF 的现代化 UI 设计
- 圆角设计 + 阴影效果
- Emoji 图标提升视觉体验
- 响应式布局适应不同屏幕

### ⚙️ **智能配置管理**
- **动态格式支持**：从 `input.ini` 和 `output.ini` 加载格式
- **自动配置保存**：JSON 格式保存用户设置
- **实时状态反馈**：彩色状态指示器
- **格式验证**：自动检查格式是否有效

### 🔄 **强大转换功能**
- **支持所有 Pandoc 格式**：80+ 输入格式，70+ 输出格式
- **智能命名**：自动添加时间戳避免重复
- **批量就绪**：为批量转换设计
- **预览功能**：转换前预览所有设置

### 🛠️ **便捷操作**
- **文件拖放就绪**（可轻松扩展）
- **文件夹浏览**：集成系统文件夹选择
- **智能格式检测**：根据文件扩展名自动建议格式
- **一键打开文件夹**：转换后快速访问结果

## 📋 支持格式

### 输入格式（从 input.ini 加载）
```
asciidoc, bibtex, biblatex, bits, commonmark, commonmark_x, creole, csljson,
csv, tsv, djot, docbook, docx, dokuwiki, endnotexml, epub, fb2, gfm,
haddock, html, ipynb, jats, jira, json, latex, markdown, markdown_mmd,
markdown_phpextra, markdown_strict, mediawiki, man, mdoc, muse, native,
odt, opml, org, pod, pptx, ris, rtf, rst, t2t, textile, tikiwiki, twiki,
typst, vimwiki, xlsx, xml
```

### 输出格式（从 output.ini 加载）
```
ansi, asciidoc, asciidoc_legacy, asciidoctor, bbcode, beamer, bibtex,
biblatex, chunkedhtml, commonmark, commonmark_x, context, csljson,
djot, docbook, docbook5, docx, dokuwiki, epub, epub2, fb2, gfm,
haddock, html, html4, icml, ipynb, jats_archiving, jats_articleauthoring,
jats_publishing, jira, json, latex, man, markdown, markdown_mmd,
markdown_phpextra, markdown_strict, markua, mediawiki, ms, muse,
native, odt, opml, opendocument, org, pdf, plain, pptx, rst, rtf,
texinfo, textile, slideous, slidy, dzslides, revealjs, s5, tei,
typst, vimdoc, xml, xwiki, zimwiki
```

## 🚀 快速开始

### 方法一：直接运行（推荐）
1. **下载** 最新的 [Release](https://github.com/northpick/pandoc-better-use/releases)
2. **解压** 到任意文件夹
3. **双击运行** `PandocConverter.exe`
4. **第一次运行** 会自动生成配置文件

### 方法二：从源代码构建
```bash
# 1. 克隆仓库
git clone https://github.com/northpick/pandoc-better-use.git

# 2. 用 Visual Studio 2022 打开项目
# 3. 确保安装了 .NET 8 SDK
# 4. 按 F5 编译运行
```

### 必需依赖
- [.NET 8 Runtime](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Pandoc](https://pandoc.org/installing.html)（程序会自动检测并提示安装）

## 📖 使用指南

### 1. 基本使用
```
1. 点击 "浏览..." 选择输入文件
2. 程序自动检测并建议输入格式
3. 选择输出目录
4. 选择输出格式
5. 点击 "🚀 开始转换"
```

### 2. 配置说明
程序生成三个配置文件：
- **pandoc_config.json** - 用户个人设置（自动管理）
- **input.ini** - 输入格式列表（可自定义）
- **output.ini** - 输出格式列表（可自定义）

### 3. 自定义格式
要添加新的格式支持：
1. 打开 `input.ini` 或 `output.ini`
2. 添加新的格式名称（每行一个）
3. 点击界面上的 "🔄 刷新格式" 按钮
4. 新格式会立即出现在下拉列表中

### 4. 示例：Markdown 转 Word
```
输入文件:  D:\docs\article.md
输入格式:  markdown (自动检测)
输出目录:  D:\output\
输出格式:  docx
输出文件:  article_20240129_143022.docx
```

## 🎯 功能演示

### 界面概览

<img width="1178" height="766" alt="UI-WPF_Modern-blue" src="https://github.com/user-attachments/assets/12abb828-ab25-44cd-9190-5bcc8203494e" />

## 🔧 技术细节

### 项目结构
```
PandocConverter/
├── 📁 bin/                    # 编译输出
├── 📁 Properties/            # 项目属性
├── 📄 App.xaml              # 应用程序资源
├── 📄 App.xaml.cs           # 应用程序逻辑
├── 📄 MainWindow.xaml       # 主界面设计
├── 📄 MainWindow.xaml.cs    # 主界面逻辑
├── 📄 input.ini             # 输入格式配置
├── 📄 output.ini            # 输出格式配置
└── 📄 pandoc_config.json    # 用户配置
```

### 主要功能实现
- **异步转换**：使用 async/await 避免界面卡顿
- **进程管理**：后台调用 Pandoc 进程
- **错误处理**：详细的错误提示和日志
- **配置持久化**：JSON 序列化保存设置
- **动态UI更新**：实时状态反馈

## 🤝 贡献指南

欢迎提交 Issue 和 Pull Request！

### 开发环境设置
```bash
1. 安装 Visual Studio 2022+
2. 安装 .NET 8 SDK
3. 克隆项目: git clone [repo-url]
4. 打开 PandocConverter.sln
5. 添加 System.Windows.Forms 引用
6. 按 F5 运行
```

### 待开发功能
- [ ] 批量文件转换
- [ ] 自定义转换模板
- [ ] 主题切换（深色/浅色）
- [ ] 转换历史记录
- [ ] 插件系统支持

## 📊 性能优化

- **轻量级**：单文件部署，无需安装
- **快速启动**：WPF 即时编译，启动迅速
- **内存友好**：异步操作，资源占用低
- **配置缓存**：格式列表缓存提升性能

## 🆚 与原始批处理版本对比

| 功能 | 批处理版本 | WPF 版本 |
|------|------------|-----------|
| 界面 | 命令行 | 图形界面 |
| 易用性 | 需要编辑INI | 点击操作 |
| 格式管理 | 固定列表 | 动态加载 |
| 错误提示 | 简单文本 | 详细弹窗 |
| 状态反馈 | 无 | 实时显示 |
| 配置保存 | INI 文件 | JSON 文件 |
| 扩展性 | 困难 | 容易 |

## 📞 支持与反馈

### 常见问题
**Q: 程序提示"未找到Pandoc"**  
A: 请先安装 [Pandoc](https://pandoc.org/installing.html)，并确保在系统PATH中

**Q: 转换失败怎么办？**  
A: 检查：
1. 输入文件是否存在
2. 格式是否匹配（如：不能将图片转为docx）
3. Pandoc版本是否支持该格式

**Q: 如何添加自定义格式？**  
A: 编辑 `input.ini` 或 `output.ini`，然后点击"刷新格式"

### 报告问题
请在 [GitHub Issues](https://github.com/yourusername/pandoc-better-use/issues) 报告：
1. 问题描述
2. 复现步骤
3. 错误截图
4. 系统环境

## 📄 许可证

MIT License - 详见 [LICENSE](LICENSE) 文件

## 🌟 致谢

- [Pandoc](https://pandoc.org/) - 强大的文档转换工具
- [.NET WPF](https://dotnet.microsoft.com/apps/desktop) - 微软桌面应用框架
- 所有贡献者和用户

---

**让文档转换变得简单而强大！** 🚀

*如果这个项目对你有帮助，请给个 Star ⭐ 支持一下！*
