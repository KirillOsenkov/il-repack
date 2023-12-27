using System;
using System.Collections;
using System.IO;
using System.Linq;
using Fasterflect;
using ILRepacking;
using Mono.Cecil;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace ILRepack.Tests
{
    class Win32ResourcesTests
    {
        [Test]
        public void Win32ResourcesInPrimary()
        {
            var logger = new TestLogger();
            var options = new RepackOptions(
                new string[]
                {
                    "/out:Win32ResourcePrimary.dll",
                    "Win32ResourceDll.dll",
                    "ClassLibrary.dll"
                });

            var repack = new ILRepacking.ILRepack(options, logger);
            repack.Repack();

            var originalBytes = ReadRsrcBytes("Win32ResourceDll.dll");
            var repackedBytes = ReadRsrcBytes("Win32ResourcePrimary.dll");

            File.WriteAllText(@"C:\temp\win32res\1.txt", GetString(originalBytes));
            File.WriteAllText(@"C:\temp\win32res\2.txt", GetString(repackedBytes));

            bool equal = originalBytes.SequenceEqual(repackedBytes);
            Assert.True(equal);

            //AssemblyDefinition assemblyDefinition = AssemblyDefinition.CreateAssembly(
            //    new AssemblyNameDefinition("1", Version.Parse("1.0.0.0")), "1.dll", ModuleKind.Dll);
            //var type = new TypeDefinition(null, "C", TypeAttributes.Class | TypeAttributes.Public);
            //assemblyDefinition.MainModule.Types.Add(type);
            //assemblyDefinition.Write(@"C:\temp\1.dll");
        }

        private static string GetString(byte[] bytes)
        {
            var result = string.Join("\r\n", bytes);
            return result;
        }

        private static byte[] ReadRsrcBytes(string filePath)
        {
            var dll = ModuleDefinition.ReadModule(filePath);
            var image = dll.GetFieldValue("Image");
            var sections = image.GetFieldValue("Sections") as Array;
            foreach (var section in sections)
            {
                string name = (string)section.GetFieldValue("Name");
                if (name == ".rsrc")
                {
                    byte[] data = (byte[])section.GetFieldValue("Data");
                    return data;
                }
            }

            return null;
        }
    }
}