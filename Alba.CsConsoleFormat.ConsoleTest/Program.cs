﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Alba.CsConsoleFormat.Generation;
using static System.ConsoleColor;
using static Alba.CsConsoleFormat.Chars;

namespace Alba.CsConsoleFormat.ConsoleTest
{
    internal class Program
    {
        public static void Main ()
        {
            new Program().Run();
            Console.WriteLine("Done!");
            if (Debugger.IsAttached)
                Console.ReadLine();
        }

        private void Run ()
        {
            /*Size consoleSize = Size.Min(ConsoleRenderer.ConsoleLargestWindowSize, new Size((25 + 1) * 7 + 1, 60));
            try {
                ConsoleRenderer.ConsoleWindowRect = new Rect(consoleSize);
                Console.BufferWidth = consoleSize.Width;
            }
            catch (Exception e) {
                var consoleRect = new Rect(Console.WindowLeft, Console.WindowTop, Console.WindowWidth, Console.WindowHeight);
                Console.WriteLine(consoleRect);
                var bufferRect = new Size(Console.BufferWidth, Console.BufferHeight);
                Console.WriteLine(bufferRect);
                Console.WriteLine(e.Message);
            }*/
            Console.OutputEncoding = Encoding.UTF8;
            Console.Title = Path.GetFileNameWithoutExtension(Console.Title);

            var data = new Data {
                Title = "Header Title",
                SubTitle = "Header SubTitle",
                Formatted = "Aaaa\nBbbb\nCccc",
                LoremIpsum = "Lo|rem ip|sum do|lor sit amet, con|sec|te|tur adi|pis|cing elit, sed do eius|mod tem|por in|ci|di|dunt ut la|bo|re et do|lo|re mag|na ali|qua. Ut enim ad mi|nim ve|ni|am, qu|is nos|trud exer|ci|ta|tion ul|lam|co la|bo|ris ni|si ut ali|quip ex ea com|mo|do con|se|quat. Du|is au|te iru|re do|lor in rep|re|hen|de|rit in vo|lup|ta|te ve|lit es|se cil|lum do|lo|re eu fu|gi|at nul|la pa|ri|a|tur. Ex|cep|te|ur sint oc|ca|e|cat cu|pi|da|tat non pro|i|dent, sunt in cul|pa qui of|fi|cia de|se|runt mol|lit anim id est la|bo|rum.",
                LoremIpsumShort = "Lo|rem ip|sum do|lor sit amet, con|sec|te|tur adi|pis|cing elit, sed do eius|mod tem|por in|ci|di|dunt ut la|bo|re et do|lo|re mag|na ali|qua. Ut enim ad mi|nim ve|ni|am, qu|is nos|trud exer|ci|ta|tion ul|lam|co la|bo|ris ni|si ut ali|quip ex ea com|mo|do con|se|quat.",
                Guid = Guid.NewGuid(),
                Date = DateTime.Now,
                Items = new List<DataItem> {
                    new DataItem {
                        Id = 1, Name = "Name 1", Value = "Value 1",
                        SubItems = new List<DataItem> {
                            new DataItem { Id = 11, Name = "Name 1.1", Value = "Value 1.1" },
                            new DataItem { Id = 12, Name = "Name 1.2", Value = "Value 1.2" },
                        }
                    },
                    new DataItem { Id = 2, Name = "Name 2", Value = "Value 2" },
                }
            };

            /*if (MemoryProfiler.IsActive) {
                MemoryProfiler.EnableAllocations();
                MemoryProfiler.Dump();
            }
            new ConsoleRenderer().RenderDocument(ReadXaml<Document>(data));
            if (MemoryProfiler.IsActive)
                MemoryProfiler.Dump();*/
            /*for (int i = 0; i < 100; i++)
                new ConsoleRenderer().RenderDocument(ReadXaml<Document>(data));*/
            /*if (MemoryProfiler.IsActive)
                MemoryProfiler.Dump();*/

            Document xamlDoc = ConsoleRenderer.ReadDocumentFromResource(GetType(), "Markup.xaml", data);
            //Document xamlDoc = ConsoleRenderer.ReadDocumentFromResource(GetType().Assembly, "Alba.CsConsoleFormat.ConsoleTest.Markup.xaml", data);
            Console.WriteLine("XAML");
            ConsoleRenderer.RenderDocument(xamlDoc);
            ConsoleRenderer.RenderDocument(xamlDoc, new HtmlRenderTarget(File.Create(@"../../Tmp/0.html"), new UTF8Encoding(false)));

            Document builtDoc = new ViewBuilder().CreateDocument(data);
            Console.WriteLine("Builder");
            ConsoleRenderer.RenderDocument(builtDoc);

            var buffer = new ConsoleBuffer(80) {
                LineCharRenderer = LineCharRenderer.Box,
                //Clip = new Rect(1, 1, 78, 30),
            };
            var rainbow = new[] {
                Black,
                DarkRed, DarkYellow, DarkGreen, DarkCyan, DarkBlue, DarkMagenta, DarkRed,
                Black,
                Red, Yellow, Green, Cyan, Blue, Magenta, Red,
            };
            /*for (int i = 0; i < 16; i++)
                buffer.FillRectangle((ConsoleColor)i, i, i, 80 - i * 2, 31 - i * 2);*/
            for (int i = 0; i < rainbow.Length; i++)
                buffer.FillBackgroundRectangle(i, i, 80 - i * 2, (rainbow.Length - i) * 2, rainbow[i]);
            buffer.DrawHorizontalLine(1, 0, 78, White);
            buffer.DrawHorizontalLine(1, 1, 78, White, LineWidth.Wide);
            buffer.DrawHorizontalLine(3, 3, 7, White);
            buffer.DrawVerticalLine(1, 1, 9, White);
            buffer.DrawVerticalLine(2, 2, 4, White);
            buffer.DrawVerticalLine(5, 0, 6, White, LineWidth.Wide);
            buffer.DrawVerticalLine(5, 0, 6, White);
            buffer.DrawVerticalLine(6, 0, 6, White);
            buffer.DrawVerticalLine(3, 0, 12, White, LineWidth.Wide);
            buffer.DrawRectangle(0, 0, 80, 32, White, LineWidth.Wide);
            buffer.FillBackgroundVerticalLine(40, 0, 32, Yellow);
            buffer.FillForegroundVerticalLine(41, 0, 32, White, FullBlock);
            buffer.FillForegroundVerticalLine(42, 0, 32, White, DarkShade);
            buffer.FillForegroundVerticalLine(43, 0, 32, White, MediumShade);
            buffer.FillForegroundVerticalLine(44, 0, 32, White, LightShade);
            buffer.DrawString(15, 15, Black, "Hello world!");
            buffer.DrawString(15, 16, White, "Hello world! Hello world! Hello world! Hello world! Hello world! Hello world!");
            //buffer.ApplyBackgroundColorMap(0, 0, buffer.Width, buffer.Height, ColorMaps.Invert);
            //buffer.ApplyForegroundColorMap(0, 0, buffer.Width, buffer.Height, ColorMaps.Invert);

            //new ConsoleRenderTarget().Render(buffer);
            //new ConsoleRenderTarget { ColorOverride = ConsoleColor.White, BgColorOverride = ConsoleColor.Black }.Render(buffer);
            new HtmlRenderTarget(File.Create(@"../../Tmp/1.html"), new UTF8Encoding(false)).Render(buffer);
            new AnsiRenderTarget(new StreamWriter(File.Create(@"../../Tmp/1.ans"), Encoding.GetEncoding("ibm437")) { NewLine = "" }).Render(buffer);
            new TextRenderTarget(File.Create(@"../../Tmp/1.txt")).Render(buffer);
            new TextRenderTarget(File.Create(@"../../Tmp/1.asc"), Encoding.GetEncoding("ibm437")).Render(buffer);

            /*var text = new TextRenderTarget();
            text.Render(buffer);
            Console.Write(text.OutputText);*/

            /*Console.WriteLine(Console.OutputEncoding);
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("■▬▲►▼◄");
            Console.WriteLine("▀▄█▌▐");
            Console.WriteLine("♠♣♥♦");
            Console.WriteLine("☺☻☼♀♂♫");
            Console.WriteLine("«»‘’‚‛“”„‟‹›");*/
            /*const string TestString1 = "«»‘’‚‛“”„‟‹›", TestString2 = "─═│║┼╪╫╬";
            foreach (EncodingInfo encodingInfo in Encoding.GetEncodings()) {
                try {
                    Encoding encoding = encodingInfo.GetEncoding();
                    bool matched1 = encoding.GetString(encoding.GetBytes(TestString1)) == TestString1;
                    bool matched2 = encoding.GetString(encoding.GetBytes(TestString2)) == TestString2;
                    Console.OutputEncoding = encoding;
                    Console.WriteLine("{0,-10}{1,-20}{2}{3} {4} {5}",
                        encodingInfo.CodePage, encodingInfo.Name, matched1 ? "+" : "-", matched2 ? "+" : "-", TestString1, TestString2);
                }
                catch {
                    Console.WriteLine("{0,-10}{1,-20}xx FAILED",
                        encodingInfo.CodePage, encodingInfo.Name);
                }
            }*/
            /*for (int i = 1; i < 10000; i += 10) {
                var sb = new StringBuilder();
                for (int j = 0; j < 10; j++)
                    sb.AppendFormat("{0,-6}{1} ", (i + j), (char)(i + j));
                Console.Write(sb);
            }*/
        }
    }

    public class Data
    {
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Formatted { get; set; }
        public string LoremIpsum { get; set; }
        public string LoremIpsumShort { get; set; }
        public Guid Guid { get; set; }
        public DateTime Date { get; set; }
        public List<DataItem> Items { get; set; }

        public static string Replace (string value, string to) => value.Replace("|", to);
        public static string Replace (string value, char to) => value.Replace('|', to);
        public static IEnumerable<string> ReplaceSplit (string value, string to) => Split(Replace(value, to));
        public static IEnumerable<string> ReplaceSplit (string value, char to) => Split(Replace(value, to));
        private static IEnumerable<string> Split (string value) => value.Select(c => c.ToString());

        public override string ToString () => GetType().Name;
    }

    public class DataItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public List<DataItem> SubItems { get; set; }

        public override string ToString () => GetType().Name;
    }

    public class ViewBuilder : DocumentBuilder
    {
        public Document CreateDocument (Data data)
        {
            var cellHeaderStroke = new LineThickness(LineWidth.Single, LineWidth.Wide, LineWidth.Single, LineWidth.Wide);
            return Create<Document>()
                .Color(White, Black)
                .AddChildren(
                    "Hello world!",
                    Create<List>(data.Items.Select(d => Create<Div>(d.Name))),
                    Create<Div>(data.Items.Select(d => d.Name + " ")),
                    Create<Grid>()
                        .AddColumns(GridLength.Auto, GridLength.Auto, GridLength.Auto)
                        .AddChildren(
                            Create<Cell>("Id").StrokeCell(cellHeaderStroke),
                            Create<Cell>("Name").StrokeCell(cellHeaderStroke),
                            Create<Cell>("Value").StrokeCell(cellHeaderStroke),
                            data.Items.Select(d => new Element[] {
                                Create<Cell>(d.Id).Color(Yellow).Align(HorizontalAlignment.Right),
                                Create<Cell>(d.Name).Color(Gray),
                                Create<Cell>(d.Value).Color(Gray),
                            })
                        ),
                    Create<Dock>()
                        .Size(width: 80).Align(HorizontalAlignment.Left).Color(Gray, Blue)
                        .AddChildren(
                            Create<Div>()
                                .DockTo(DockTo.Left).Size(width: 20).Margin(1, 1, 0, 1).Padding(1)
                                .Color(bgColor: DarkBlue).WrapText(TextWrapping.CharWrap)
                                .AddChildren(
                                    CreateText("Char wrap\n\n").Color(White),
                                    Data.Replace(data.LoremIpsum, "")
                                ),
                            Create<Div>()
                                .DockTo(DockTo.Top).Size(height: 10).Margin(1, 1, 1, 0).Padding(1)
                                .Color(bgColor: DarkBlue)
                                .AddChildren(
                                    CreateText("Word wrap with spaces\n\n").Color(White),
                                    Data.Replace(data.LoremIpsum, "")
                                ),
                            Create<Div>()
                                .DockTo(DockTo.Right).Size(width: 20).Margin(0, 1, 1, 1).Padding(1)
                                .Color(bgColor: DarkBlue)
                                .AddChildren(
                                    CreateText("Word wrap with zero-width spaces\n\n").Color(White),
                                    Data.Replace(data.LoremIpsum, ZeroWidthSpace)
                                ),
                            Create<Div>()
                                .DockTo(DockTo.Bottom).Size(height: 10).Margin(1, 0, 1, 1).Padding(1)
                                .Color(bgColor: DarkBlue)
                                .AddChildren(
                                    CreateText("Word wrap with no-break spaces\n\n").Color(White),
                                    Data.Replace(data.LoremIpsum, NoBreakSpace)
                                ),
                            Create<Border>()
                                .Margin(1).Padding(1)
                                .Color(bgColor: DarkCyan)
                                .Shadow(new Thickness(-1, -1, 1, 1)).Stroke(LineThickness.Single)
                                .AddChildren(
                                    CreateText("Word wrap with soft hyphens\n\n").Color(White),
                                    Data.Replace(data.LoremIpsum, SoftHyphen)
                                )
                        ),
                    CreateText(),
                    Create<Canvas>()
                        .Size(width: 80, height: 43).Align(HorizontalAlignment.Left).Color(Gray, Blue)
                        .AddChildren(
                            Create<Div>()
                                .At(left: 1, top: 1).Size(width: 38, height: 20).Padding(1)
                                .Color(bgColor: DarkBlue).WrapText(TextWrapping.CharWrap)
                                .AddChildren(
                                    CreateText("Char wrap\n\n").Color(White),
                                    Data.Replace(data.LoremIpsum, "")
                                ),
                            Create<Div>()
                                .At(left: 1, bottom: 1).Size(width: 38, height: 20).Padding(1)
                                .Color(bgColor: DarkBlue)
                                .AddChildren(
                                    CreateText("Word wrap with spaces\n\n").Color(White),
                                    Data.Replace(data.LoremIpsum, "")
                                ),
                            Create<Div>()
                                .At(right: 1, top: 1).Size(width: 38, height: 20).Padding(1)
                                .Color(bgColor: DarkBlue)
                                .AddChildren(
                                    CreateText("Word wrap with zero-width spaces\n\n").Color(White),
                                    Data.Replace(data.LoremIpsum, ZeroWidthSpace)
                                ),
                            Create<Div>()
                                .At(right: 1, bottom: 1).Size(width: 38, height: 20).Padding(1)
                                .Color(bgColor: DarkBlue)
                                .AddChildren(
                                    CreateText("Word wrap with no-break spaces\n\n").Color(White),
                                    Data.Replace(data.LoremIpsum, NoBreakSpace)
                                ),
                            Create<Border>()
                                .At(left: 21, top: 11).Size(width: 38, height: 20).Padding(1)
                                .Color(bgColor: DarkCyan)
                                .Shadow(new Thickness(-1, -1, 1, 1)).Stroke(LineThickness.Single)
                                .AddChildren(
                                    CreateText("Word wrap with soft hyphens\n\n").Color(White),
                                    Data.Replace(data.LoremIpsum, SoftHyphen)
                                )
                        )
                );
        }
    }
}