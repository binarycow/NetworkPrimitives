﻿#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using NetworkPrimitives.Utilities;
using NUnit.Framework;

namespace NetworkPrimitives.Tests.Common
{
    public record ByteParseTestCase(
        string Input,
        byte ExpectedValue,
        int ExpectedLength
    )
    {
        public ByteParseTestCase(string invalidInput) : this(invalidInput, 0, 0)
        {
        }
    }


    public class ParsingTests
    {
        public static IReadOnlyList<ByteParseTestCase> SuccessfulTestCases = new ByteParseTestCase[]
        {
            new("0", 0, 1),
            new("1", 1, 1),
            new("2", 2, 1),
            new("3", 3, 1),
            new("4", 4, 1),
            new("5", 5, 1),
            new("6", 6, 1),
            new("7", 7, 1),
            new("8", 8, 1),
            new("9", 9, 1),
            new("10", 10, 2),
            new("11", 11, 2),
            new("12", 12, 2),
            new("13", 13, 2),
            new("14", 14, 2),
            new("15", 15, 2),
            new("16", 16, 2),
            new("17", 17, 2),
            new("18", 18, 2),
            new("19", 19, 2),
            new("20", 20, 2),
            new("21", 21, 2),
            new("22", 22, 2),
            new("23", 23, 2),
            new("24", 24, 2),
            new("25", 25, 2),
            new("26", 26, 2),
            new("27", 27, 2),
            new("28", 28, 2),
            new("29", 29, 2),
            new("30", 30, 2),
            new("31", 31, 2),
            new("32", 32, 2),
            new("33", 33, 2),
            new("34", 34, 2),
            new("35", 35, 2),
            new("36", 36, 2),
            new("37", 37, 2),
            new("38", 38, 2),
            new("39", 39, 2),
            new("40", 40, 2),
            new("41", 41, 2),
            new("42", 42, 2),
            new("43", 43, 2),
            new("44", 44, 2),
            new("45", 45, 2),
            new("46", 46, 2),
            new("47", 47, 2),
            new("48", 48, 2),
            new("49", 49, 2),
            new("50", 50, 2),
            new("51", 51, 2),
            new("52", 52, 2),
            new("53", 53, 2),
            new("54", 54, 2),
            new("55", 55, 2),
            new("56", 56, 2),
            new("57", 57, 2),
            new("58", 58, 2),
            new("59", 59, 2),
            new("60", 60, 2),
            new("61", 61, 2),
            new("62", 62, 2),
            new("63", 63, 2),
            new("64", 64, 2),
            new("65", 65, 2),
            new("66", 66, 2),
            new("67", 67, 2),
            new("68", 68, 2),
            new("69", 69, 2),
            new("70", 70, 2),
            new("71", 71, 2),
            new("72", 72, 2),
            new("73", 73, 2),
            new("74", 74, 2),
            new("75", 75, 2),
            new("76", 76, 2),
            new("77", 77, 2),
            new("78", 78, 2),
            new("79", 79, 2),
            new("80", 80, 2),
            new("81", 81, 2),
            new("82", 82, 2),
            new("83", 83, 2),
            new("84", 84, 2),
            new("85", 85, 2),
            new("86", 86, 2),
            new("87", 87, 2),
            new("88", 88, 2),
            new("89", 89, 2),
            new("90", 90, 2),
            new("91", 91, 2),
            new("92", 92, 2),
            new("93", 93, 2),
            new("94", 94, 2),
            new("95", 95, 2),
            new("96", 96, 2),
            new("97", 97, 2),
            new("98", 98, 2),
            new("99", 99, 2),
            new("100", 100, 3),
            new("101", 101, 3),
            new("102", 102, 3),
            new("103", 103, 3),
            new("104", 104, 3),
            new("105", 105, 3),
            new("106", 106, 3),
            new("107", 107, 3),
            new("108", 108, 3),
            new("109", 109, 3),
            new("110", 110, 3),
            new("111", 111, 3),
            new("112", 112, 3),
            new("113", 113, 3),
            new("114", 114, 3),
            new("115", 115, 3),
            new("116", 116, 3),
            new("117", 117, 3),
            new("118", 118, 3),
            new("119", 119, 3),
            new("120", 120, 3),
            new("121", 121, 3),
            new("122", 122, 3),
            new("123", 123, 3),
            new("124", 124, 3),
            new("125", 125, 3),
            new("126", 126, 3),
            new("127", 127, 3),
            new("128", 128, 3),
            new("129", 129, 3),
            new("130", 130, 3),
            new("131", 131, 3),
            new("132", 132, 3),
            new("133", 133, 3),
            new("134", 134, 3),
            new("135", 135, 3),
            new("136", 136, 3),
            new("137", 137, 3),
            new("138", 138, 3),
            new("139", 139, 3),
            new("140", 140, 3),
            new("141", 141, 3),
            new("142", 142, 3),
            new("143", 143, 3),
            new("144", 144, 3),
            new("145", 145, 3),
            new("146", 146, 3),
            new("147", 147, 3),
            new("148", 148, 3),
            new("149", 149, 3),
            new("150", 150, 3),
            new("151", 151, 3),
            new("152", 152, 3),
            new("153", 153, 3),
            new("154", 154, 3),
            new("155", 155, 3),
            new("156", 156, 3),
            new("157", 157, 3),
            new("158", 158, 3),
            new("159", 159, 3),
            new("160", 160, 3),
            new("161", 161, 3),
            new("162", 162, 3),
            new("163", 163, 3),
            new("164", 164, 3),
            new("165", 165, 3),
            new("166", 166, 3),
            new("167", 167, 3),
            new("168", 168, 3),
            new("169", 169, 3),
            new("170", 170, 3),
            new("171", 171, 3),
            new("172", 172, 3),
            new("173", 173, 3),
            new("174", 174, 3),
            new("175", 175, 3),
            new("176", 176, 3),
            new("177", 177, 3),
            new("178", 178, 3),
            new("179", 179, 3),
            new("180", 180, 3),
            new("181", 181, 3),
            new("182", 182, 3),
            new("183", 183, 3),
            new("184", 184, 3),
            new("185", 185, 3),
            new("186", 186, 3),
            new("187", 187, 3),
            new("188", 188, 3),
            new("189", 189, 3),
            new("190", 190, 3),
            new("191", 191, 3),
            new("192", 192, 3),
            new("193", 193, 3),
            new("194", 194, 3),
            new("195", 195, 3),
            new("196", 196, 3),
            new("197", 197, 3),
            new("198", 198, 3),
            new("199", 199, 3),
            new("200", 200, 3),
            new("201", 201, 3),
            new("202", 202, 3),
            new("203", 203, 3),
            new("204", 204, 3),
            new("205", 205, 3),
            new("206", 206, 3),
            new("207", 207, 3),
            new("208", 208, 3),
            new("209", 209, 3),
            new("210", 210, 3),
            new("211", 211, 3),
            new("212", 212, 3),
            new("213", 213, 3),
            new("214", 214, 3),
            new("215", 215, 3),
            new("216", 216, 3),
            new("217", 217, 3),
            new("218", 218, 3),
            new("219", 219, 3),
            new("220", 220, 3),
            new("221", 221, 3),
            new("222", 222, 3),
            new("223", 223, 3),
            new("224", 224, 3),
            new("225", 225, 3),
            new("226", 226, 3),
            new("227", 227, 3),
            new("228", 228, 3),
            new("229", 229, 3),
            new("230", 230, 3),
            new("231", 231, 3),
            new("232", 232, 3),
            new("233", 233, 3),
            new("234", 234, 3),
            new("235", 235, 3),
            new("236", 236, 3),
            new("237", 237, 3),
            new("238", 238, 3),
            new("239", 239, 3),
            new("240", 240, 3),
            new("241", 241, 3),
            new("242", 242, 3),
            new("243", 243, 3),
            new("244", 244, 3),
            new("245", 245, 3),
            new("246", 246, 3),
            new("247", 247, 3),
            new("248", 248, 3),
            new("249", 249, 3),
            new("250", 250, 3),
            new("251", 251, 3),
            new("252", 252, 3),
            new("253", 253, 3),
            new("254", 254, 3),
            new("255", 255, 3),
        };

        public static IReadOnlyList<ByteParseTestCase> FailedTestCases = new ByteParseTestCase[]
        {
            new("-30"),
            new("-29"),
            new("-28"),
            new("-27"),
            new("-26"),
            new("-25"),
            new("-24"),
            new("-23"),
            new("-22"),
            new("-21"),
            new("-20"),
            new("-19"),
            new("-18"),
            new("-17"),
            new("-16"),
            new("-15"),
            new("-14"),
            new("-13"),
            new("-12"),
            new("-11"),
            new("-10"),
            new("-9"),
            new("-8"),
            new("-7"),
            new("-6"),
            new("-5"),
            new("-4"),
            new("-3"),
            new("-2"),
            new("-1"),
            new("256"),
            new("257"),
            new("258"),
            new("259"),
            new("260"),
            new("261"),
            new("262"),
            new("263"),
            new("264"),
            new("265"),
            new("266"),
            new("267"),
            new("268"),
            new("269"),
            new("270"),
            new("271"),
            new("272"),
            new("273"),
            new("274"),
            new("275"),
            new("276"),
            new("277"),
            new("278"),
            new("279"),
            new("280"),
            new("281"),
            new("282"),
            new("283"),
        };

        [Test]
        [TestCaseSource(nameof(SuccessfulTestCases))]
        [Category("parsing")]
        public void SuccessfulByteParse(ByteParseTestCase testCase)
        {
            var (input, expectedValue, expectedLength) = testCase;
            var span = input.AsSpan();
            var charsRead = 0;
            Assert.That(span.TryParseByte(ref charsRead, out var value));
            var remainingString = span.GetString();
            var expectedRemainingString = input.Substring(expectedLength);
            Assert.That(value, Is.EqualTo(expectedValue));
            Assert.That(value, Is.EqualTo(expectedValue));
            Assert.That(charsRead, Is.EqualTo(expectedLength));
            Assert.That(remainingString, Is.EqualTo(expectedRemainingString));
        }


        [Test]
        [TestCaseSource(nameof(FailedTestCases))]
        [Category("parsing")]
        public void FailedByteParse(ByteParseTestCase testCase)
        {
            var input = testCase.Input;
            var charsRead = 0;
            var span = input.AsSpan();
            Assert.That(span.TryParseByte(ref charsRead, out var value), Is.False);
            var remainingString = span.GetString();
            Assert.That(charsRead, Is.Zero);
            Assert.That(remainingString, Is.EqualTo(input));
            Assert.That(value, Is.Zero);
        }
    }
}