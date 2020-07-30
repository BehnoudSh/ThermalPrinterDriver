using System;
using System.IO.Ports;
using System.Threading;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections;
using System.Text;

namespace VVMMainProject
{
    #region Enums
    public enum _Language
    {
        English,
        Persian
    }
    public enum _UnderLine
    {
        OneDotUnderLine,
        TwoDotsUnderLine,
        Reset
    }
    public enum _Bold
    {
        Bold,
        NotBold
    }
    public enum _FontUnderLine
    {
        FontSize,
        FontSizeDefault,
        FontSizeUnderLine
    }
    public enum _ErrorLists
    {
        Normal,
        PresenterClamp,
        PaperDetectError,
        AutoCutterError,
        PresenterEjectionError,
        PaperEnd,
        ThermalHeadCoverOpen,
        PaperNearEnd,
        FatalError
    }
    #endregion

    class NipponPrinter : Component
    {
        #region Variables
        SerialPort serial;
        //bool CTS;
        #endregion

        #region Constructor
        string temp = "";
        public NipponPrinter(string com_port)
        {
            IO_Control.Output_Pin(IO_Control.pins.printer_serial_key, IO_Control.pin_status.pin_set);


            IO_Control.Output_Pin(IO_Control.pins.printer_power_key, IO_Control.pin_status.pin_reset);
            Thread.Sleep(200);
            IO_Control.Output_Pin(IO_Control.pins.printer_power_key, IO_Control.pin_status.pin_set);
            Thread.Sleep(2000);

            serial = new SerialPort(com_port, 115200, Parity.None, 8);
            serial.Open();

            byte[] SoftwareReset = { 0x11 };//SoftwareReset
            for (int i = 0; i < 3; i++)
            {
                serial.Write(SoftwareReset, 0, SoftwareReset.Length);
                Thread.Sleep(2000);
            }

            byte[] PrinterSelect = { 0x1B, 0x3D, 0x01 };//PrinterSelect
            serial.Write(PrinterSelect, 0, PrinterSelect.Length);

            byte[] PrinterInitilization = { 0x1B, 0x40 };//PrinterInitilization
            serial.Write(PrinterInitilization, 0, PrinterInitilization.Length);

            byte[] BackFeed = { 0x1B, 0x42, 0x0F };//In order to avoid paper jam
            serial.Write(BackFeed, 0, BackFeed.Length);

            byte[] PrinterInformation = { 0x1B, 0x73, 0x02 };//PrinterInformation
            serial.Write(PrinterInformation, 0, PrinterInformation.Length);
            temp = serial.ReadExisting();

            byte[] IranSystem = { 0x1B, 0x74, 0x07, 0x0D, 0x0A };//CodePage IranSystem
            serial.Write(IranSystem, 0, IranSystem.Length);

            byte[] PrintDensity = { 0x1D, 0x7E, 0x87 };//PrintDensity
            serial.Write(PrintDensity, 0, PrintDensity.Length);

            byte[] Centering = { 0x1B, 0x61, 0x01 };//Centering
            serial.Write(Centering, 0, Centering.Length);

            byte[] PageBufferClear = { 0x18 };//PageBufferClear
            serial.Write(PageBufferClear, 0, PageBufferClear.Length);

            byte[] ManualEject = { 0x1B, 0x68, 0x02 };//ManualEject
            serial.Write(ManualEject, 0, ManualEject.Length);

            //CTS = serial.CtsHolding;
        }
        #endregion

        #region MainPrintingMethods

        public void PrintWords(string input_string, _Language select_language)
        {
            char[] InputArray;
            //CTS = serial.CtsHolding;
            if (select_language == _Language.English)
            {
                serial.Write(input_string);
            }
            else if (select_language == _Language.Persian)
            {
                char[] f = input_string.ToCharArray();
                Array.Reverse(f);

                input_string = "";

                for (byte k = 0; k < f.Length; k++)
                {
                    input_string += f[k].ToString();
                }

                string[] Whole_Sentence_Array = input_string.Split(' ');//Splitting the Sentence

                foreach (string str in Whole_Sentence_Array)
                {
                    InputArray = str.ToCharArray();//Splitting the Input String

                    byte[] temp = new byte[InputArray.Length];
                    UInt16 counter = 0;

                    for (byte k = 0; k < InputArray.Length; k++)
                    {
                        #region FarsiCharactersSwitchCase
                        switch ((int)InputArray[k])
                        {


                            case 33://!
                                {
                                    temp[counter] = 33;
                                    counter++;
                                    break;
                                }
                            case 34://"
                                {
                                    temp[counter] = 34;
                                    counter++;
                                    break;
                                }
                            case 35://#
                                {
                                    temp[counter] = 35;
                                    counter++;
                                    break;
                                }
                            case 36://$
                                {
                                    temp[counter] = 36;
                                    counter++;
                                    break;
                                }
                            case 37://%
                                {
                                    temp[counter] = 37;
                                    counter++;
                                    break;
                                }
                            case 38://&
                                {
                                    temp[counter] = 38;
                                    counter++;
                                    break;
                                }
                            case 39://'
                                {
                                    temp[counter] = 39;
                                    counter++;
                                    break;
                                }
                            case 40://(
                                {
                                    temp[counter] = 40;
                                    counter++;
                                    break;
                                }
                            case 41://)
                                {
                                    temp[counter] = 41;
                                    counter++;
                                    break;
                                }
                            case 42://*
                                {
                                    temp[counter] = 42;
                                    counter++;
                                    break;
                                }
                            case 43://+
                                {
                                    temp[counter] = 43;
                                    counter++;
                                    break;
                                }
                            case 44://,
                                {
                                    temp[counter] = 138;
                                    counter++;
                                    break;
                                }
                            case 45://-
                                {
                                    temp[counter] = 139;
                                    counter++;
                                    break;
                                }
                            case 46://.
                                {
                                    temp[counter] = 46;
                                    counter++;
                                    break;
                                }
                            case 47:// /
                                {
                                    temp[counter] = 47;
                                    counter++;
                                    break;
                                }
                            case 58://:
                                {
                                    temp[counter] = 58;
                                    counter++;
                                    break;
                                }
                            case 59://;
                                {
                                    temp[counter] = 59;
                                    counter++;
                                    break;
                                }
                            case 60://<
                                {
                                    temp[counter] = 60;
                                    counter++;
                                    break;
                                }
                            case 61://=
                                {
                                    temp[counter] = 61;
                                    counter++;
                                    break;
                                }
                            case 62://>
                                {
                                    temp[counter] = 62;
                                    counter++;
                                    break;
                                }
                            case 1567://?
                                {
                                    temp[counter] = 140;
                                    counter++;
                                    break;
                                }
                            case 64://@
                                {
                                    temp[counter] = 64;
                                    counter++;
                                    break;
                                }
                            case 91://[
                                {
                                    temp[counter] = 91;
                                    counter++;
                                    break;
                                }
                            case 92://\
                                {
                                    temp[counter] = 92;
                                    counter++;
                                    break;
                                }
                            case 93://]
                                {
                                    temp[counter] = 93;
                                    counter++;
                                    break;
                                }
                            case 94://^
                                {
                                    temp[counter] = 94;
                                    counter++;
                                    break;
                                }
                            case 95://_
                                {
                                    temp[counter] = 95;
                                    counter++;
                                    break;
                                }
                            case 123://{
                                {
                                    temp[counter] = 123;
                                    counter++;
                                    break;
                                }
                            case 124://|
                                {
                                    temp[counter] = 124;
                                    counter++;
                                    break;
                                }
                            case 125://}
                                {
                                    temp[counter] = 125;
                                    counter++;
                                    break;
                                }
                            case 126://~
                                {
                                    temp[counter] = 126;
                                    counter++;
                                    break;
                                }
                            case 1570://آ
                                {
                                    temp[counter] = 141;
                                    counter++;
                                    break;
                                }
                            case 1575://ا 
                                {
                                    if (k == str.Length - 1)
                                    {
                                        temp[counter] = 144;
                                        counter++;
                                    }
                                    else
                                    {
                                        switch (InputArray[k + 1])
                                        {
                                            case 'ب':
                                            case 'پ':
                                            case 'ت':
                                            case 'ث':
                                            case 'ج':
                                            case 'چ':
                                            case 'ح':
                                            case 'خ':
                                            case 'س':
                                            case 'ش':
                                            case 'ص':
                                            case 'ض':
                                            case 'ط':
                                            case 'ظ':
                                            case 'ع':
                                            case 'غ':
                                            case 'ف':
                                            case 'ق':
                                            case 'ک':
                                            case 'گ':
                                            case 'ل':
                                            case 'م':
                                            case 'ن':
                                            case 'ه':
                                            case 'ی':
                                                {
                                                    temp[counter] = 145;
                                                    counter++;
                                                    break;
                                                }
                                            default:
                                                temp[counter] = 144;
                                                counter++;
                                                break;
                                        }

                                    }

                                    break;
                                }
                            case 1576://ب
                                {
                                    if (k == 0)
                                    {
                                        temp[counter] = 146;
                                        counter++;
                                    }
                                    else
                                    {
                                        temp[counter] = 147;
                                        counter++;
                                    }

                                    break;
                                }
                            case 1662://پ   
                                {
                                    if (k == 0)
                                    {
                                        temp[counter] = 148;
                                        counter++;
                                    }
                                    else
                                    {
                                        temp[counter] = 149;
                                        counter++;
                                    }

                                    break;
                                }
                            case 1578://ت   
                                {
                                    if (k == 0)
                                    {
                                        temp[counter] = 150;
                                        counter++;
                                    }
                                    else
                                    {
                                        temp[counter] = 151;
                                        counter++;
                                    }

                                    break;
                                }
                            case 1579://ث   
                                {
                                    if (k == 0)
                                    {
                                        temp[counter] = 152;
                                        counter++;
                                    }
                                    else
                                    {
                                        temp[counter] = 153;
                                        counter++;
                                    }

                                    break;
                                }
                            case 1580://ج  
                                {
                                    if (k == 0)
                                    {
                                        temp[counter] = 154;
                                        counter++;
                                    }
                                    else
                                    {
                                        temp[counter] = 155;
                                        counter++;
                                    }

                                    break;
                                }
                            case 1670://چ  
                                {
                                    if (k == 0)
                                    {
                                        temp[counter] = 156;
                                        counter++;
                                    }
                                    else
                                    {
                                        temp[counter] = 157;
                                        counter++;
                                    }

                                    break;
                                }
                            case 1581://ح  
                                {
                                    if (k == 0)
                                    {
                                        temp[counter] = 158;
                                        counter++;
                                    }
                                    else
                                    {
                                        temp[counter] = 159;
                                        counter++;
                                    }

                                    break;
                                }
                            case 1582://خ  
                                {
                                    if (k == 0)
                                    {
                                        temp[counter] = 160;
                                        counter++;
                                    }
                                    else
                                    {
                                        temp[counter] = 161;
                                        counter++;
                                    }

                                    break;
                                }
                            case 1583://د
                                {
                                    temp[counter] = 162;
                                    counter++;
                                    break;
                                }
                            case 1584://ذ
                                {
                                    temp[counter] = 163;
                                    counter++;
                                    break;
                                }
                            case 1585://ر
                                {
                                    temp[counter] = 164;
                                    counter++;
                                    break;
                                }
                            case 1586://ز
                                {
                                    temp[counter] = 165;
                                    counter++;
                                    break;
                                }
                            case 1688://ژ
                                {
                                    temp[counter] = 166;
                                    counter++;
                                    break;
                                }

                            case 1587://س
                                {
                                    if (k == 0)
                                    {
                                        temp[counter] = 167;
                                        counter++;
                                    }
                                    else
                                    {
                                        temp[counter] = 168;
                                        counter++;
                                    }

                                    break;
                                }

                            case 1588://ش
                                {
                                    if (k == 0)
                                    {
                                        temp[counter] = 169;
                                        counter++;
                                    }
                                    else
                                    {
                                        temp[counter] = 170;
                                        counter++;
                                    }

                                    break;
                                }

                            case 1589://ص
                                {
                                    if (k == 0)
                                    {
                                        temp[counter] = 171;
                                        counter++;
                                    }
                                    else
                                    {
                                        temp[counter] = 172;
                                        counter++;
                                    }

                                    break;
                                }
                            case 1590://ض
                                {
                                    if (k == 0)
                                    {
                                        temp[counter] = 173;
                                        counter++;
                                    }
                                    else
                                    {
                                        temp[counter] = 174;
                                        counter++;
                                    }

                                    break;
                                }


                            case 1591://ط
                                {
                                    temp[counter] = 175;
                                    counter++;
                                    break;
                                }
                            case 1592://ظ
                                {
                                    temp[counter] = 224;
                                    counter++;
                                    break;
                                }

                            case 1593://ع
                                {
                                    if (k == 0)
                                    {
                                        switch (InputArray[k + 1])
                                        {
                                            case 'ب':
                                            case 'پ':
                                            case 'ت':
                                            case 'ث':
                                            case 'ج':
                                            case 'چ':
                                            case 'ح':
                                            case 'خ':
                                            case 'س':
                                            case 'ش':
                                            case 'ص':
                                            case 'ض':
                                            case 'ط':
                                            case 'ظ':
                                            case 'ع':
                                            case 'غ':
                                            case 'ف':
                                            case 'ق':
                                            case 'ک':
                                            case 'گ':
                                            case 'ل':
                                            case 'م':
                                            case 'ن':
                                            case 'ه':
                                            case 'ی':
                                                {
                                                    temp[counter] = 226;
                                                    counter++;
                                                    break;
                                                }
                                            default:
                                                temp[counter] = 225;
                                                counter++;
                                                break;

                                        }
                                    }
                                    else if ((k == str.Length - 1))
                                    {
                                        temp[counter] = 228;
                                        counter++;
                                    }
                                    else
                                    {
                                        switch (InputArray[k + 1])
                                        {
                                            case 'ب':
                                            case 'پ':
                                            case 'ت':
                                            case 'ث':
                                            case 'ج':
                                            case 'چ':
                                            case 'ح':
                                            case 'خ':
                                            case 'س':
                                            case 'ش':
                                            case 'ص':
                                            case 'ض':
                                            case 'ط':
                                            case 'ظ':
                                            case 'ع':
                                            case 'غ':
                                            case 'ف':
                                            case 'ق':
                                            case 'ک':
                                            case 'گ':
                                            case 'ل':
                                            case 'م':
                                            case 'ن':
                                            case 'ه':
                                            case 'ی':
                                                {
                                                    temp[counter] = 227;
                                                    counter++;
                                                    break;
                                                }
                                            default:
                                                temp[counter] = 228;
                                                counter++;
                                                break;

                                        }
                                    }
                                    break;
                                }

                            case 1594://غ
                                {
                                    if (k == 0)
                                    {
                                        switch (InputArray[k + 1])
                                        {
                                            case 'ب':
                                            case 'پ':
                                            case 'ت':
                                            case 'ث':
                                            case 'ج':
                                            case 'چ':
                                            case 'ح':
                                            case 'خ':
                                            case 'س':
                                            case 'ش':
                                            case 'ص':
                                            case 'ض':
                                            case 'ط':
                                            case 'ظ':
                                            case 'ع':
                                            case 'غ':
                                            case 'ف':
                                            case 'ق':
                                            case 'ک':
                                            case 'گ':
                                            case 'ل':
                                            case 'م':
                                            case 'ن':
                                            case 'ه':
                                            case 'ی':
                                                {
                                                    temp[counter] = 230;
                                                    counter++;
                                                    break;
                                                }
                                            default:
                                                temp[counter] = 229;
                                                counter++;
                                                break;

                                        }
                                    }
                                    else if ((k == str.Length - 1))
                                    {
                                        temp[counter] = 232;
                                        counter++;
                                    }
                                    else
                                    {
                                        switch (InputArray[k + 1])
                                        {
                                            case 'ب':
                                            case 'پ':
                                            case 'ت':
                                            case 'ث':
                                            case 'ج':
                                            case 'چ':
                                            case 'ح':
                                            case 'خ':
                                            case 'س':
                                            case 'ش':
                                            case 'ص':
                                            case 'ض':
                                            case 'ط':
                                            case 'ظ':
                                            case 'ع':
                                            case 'غ':
                                            case 'ف':
                                            case 'ق':
                                            case 'ک':
                                            case 'گ':
                                            case 'ل':
                                            case 'م':
                                            case 'ن':
                                            case 'ه':
                                            case 'ی':
                                                {
                                                    temp[counter] = 231;
                                                    counter++;
                                                    break;
                                                }
                                            default:
                                                temp[counter] = 232;
                                                counter++;
                                                break;

                                        }
                                    }
                                    break;
                                }



                            case 1601://ف
                                {
                                    if (k == 0)
                                    {
                                        temp[counter] = 233;
                                        counter++;
                                    }
                                    else
                                    {
                                        temp[counter] = 234;
                                        counter++;
                                    }

                                    break;
                                }


                            case 1602://ق
                                {
                                    if (k == 0)
                                    {
                                        temp[counter] = 235;
                                        counter++;
                                    }
                                    else
                                    {
                                        temp[counter] = 236;
                                        counter++;
                                    }

                                    break;
                                }

                            case 1705://ک
                                {
                                    if (k == 0)
                                    {
                                        temp[counter] = 237;
                                        counter++;
                                    }
                                    else
                                    {
                                        temp[counter] = 238;
                                        counter++;
                                    }

                                    break;
                                }



                            case 1711://گ
                                {
                                    if (k == 0)
                                    {
                                        temp[counter] = 239;
                                        counter++;
                                    }
                                    else
                                    {
                                        temp[counter] = 240;
                                        counter++;
                                    }

                                    break;
                                }

                            case 1604://ل
                                {
                                    if (k == 0)
                                    {
                                        temp[counter] = 241;
                                        counter++;
                                    }
                                    else
                                    {
                                        temp[counter] = 243;
                                        counter++;
                                    }

                                    break;
                                }

                            case 1605://م
                                {
                                    if (k == 0)
                                    {
                                        temp[counter] = 244;
                                        counter++;

                                    }

                                    else
                                    {
                                        temp[counter] = 245;
                                        counter++;

                                    }

                                    break;
                                }

                            case 1606://ن
                                {
                                    if (k == 0)
                                    {
                                        temp[counter] = 246;
                                        counter++;
                                    }
                                    else
                                    {
                                        temp[counter] = 247;
                                        counter++;
                                    }
                                    break;
                                }

                            case 1608://و
                                {
                                    temp[counter] = 248;
                                    counter++;
                                    break;
                                }

                            case 1607://ه
                                {
                                    if (k == 0)//0 Charactere Akhar
                                    {
                                        temp[counter] = 249;
                                        counter++;
                                    }
                                    else if (k == str.Length - 1)
                                    {
                                        temp[counter] = 251;
                                        counter++;
                                    }
                                    else
                                    {
                                        switch (InputArray[k + 1])
                                        {
                                            case 'ر':
                                            case 'ز':
                                            case 'ژ':
                                            case 'ا':
                                            case 'آ':
                                            case 'د':
                                            case 'و':
                                            case 'ذ':
                                                {
                                                    temp[counter] = 251;
                                                    counter++;
                                                    break;
                                                }
                                            default:
                                                temp[counter] = 250;
                                                counter++;
                                                break;
                                        }
                                    }
                                    break;
                                }
                            case 1740://ی
                                {
                                    if (InputArray.Length == 1 && k == 0)//ی تنها
                                    {
                                        temp[counter] = 253;
                                        counter++;
                                        break;
                                    }

                                    else if (k == 0 && InputArray.Length >= 2)
                                    {
                                        switch (InputArray[k + 1])
                                        {
                                            case 'ب':
                                            case 'پ':
                                            case 'ت':
                                            case 'ث':
                                            case 'ج':
                                            case 'چ':
                                            case 'ح':
                                            case 'خ':
                                            case 'س':
                                            case 'ش':
                                            case 'ص':
                                            case 'ض':
                                            case 'ط':
                                            case 'ظ':
                                            case 'ع':
                                            case 'غ':
                                            case 'ف':
                                            case 'ق':
                                            case 'ک':
                                            case 'گ':
                                            case 'ل':
                                            case 'م':
                                            case 'ن':
                                            case 'ه':
                                            case 'ی':
                                                {
                                                    temp[counter] = 252;
                                                    counter++;
                                                    break;
                                                }
                                            default:
                                                temp[counter] = 253;
                                                counter++;
                                                break;

                                        }
                                    }
                                    else
                                    {
                                        temp[counter] = 254;
                                        counter++;
                                    }

                                    break;
                                }

                            case 32:// 
                                {
                                    temp[counter] = 32;
                                    counter++;
                                    break;
                                }
                            default:
                                break;
                        }
                        #endregion
                    }

                    serial.Write(temp, 0, temp.Length);
                    InputArray = null;
                    temp = null;
                    serial.Write(" ");

                }
            }
        }
        public void PrintNumbers(string input_string, _Language select_language)
        {
            char[] InputArray = new char[] { };
            //CTS = serial.CtsHolding;
            if (select_language == _Language.English)//English
            {
                serial.WriteLine(input_string);
            }
            else if (select_language == _Language.Persian)//Farsi
            {
                InputArray = input_string.ToCharArray();//Splitting the Input Number

                byte[] temp = new byte[InputArray.Length + 2];
                UInt16 counter = 0;

                foreach (char ch in InputArray)
                {
                    #region FarsiNumbers
                    switch ((int)ch)
                    {
                        case 32:// 
                            {
                                temp[counter] = 32;
                                counter++;
                                break;
                            }

                        case 33://!
                            {
                                temp[counter] = 33;
                                counter++;
                                break;
                            }
                        case 34://"
                            {
                                temp[counter] = 34;
                                counter++;
                                break;
                            }
                        case 35://#
                            {
                                temp[counter] = 35;
                                counter++;
                                break;
                            }
                        case 36://$
                            {
                                temp[counter] = 36;
                                counter++;
                                break;
                            }
                        case 37://%
                            {
                                temp[counter] = 37;
                                counter++;
                                break;
                            }
                        case 38://&
                            {
                                temp[counter] = 38;
                                counter++;
                                break;
                            }
                        case 39://'
                            {
                                temp[counter] = 39;
                                counter++;
                                break;
                            }
                        case 40://(
                            {
                                temp[counter] = 40;
                                counter++;
                                break;
                            }
                        case 41://)
                            {
                                temp[counter] = 41;
                                counter++;
                                break;
                            }
                        case 42://*
                            {
                                temp[counter] = 42;
                                counter++;
                                break;
                            }
                        case 43://+
                            {
                                temp[counter] = 43;
                                counter++;
                                break;
                            }
                        case 44://,
                            {
                                temp[counter] = 138;
                                counter++;
                                break;
                            }
                        case 45://-
                            {
                                temp[counter] = 139;
                                counter++;
                                break;
                            }
                        case 46://.
                            {
                                temp[counter] = 46;
                                counter++;
                                break;
                            }
                        case 47:// /
                            {
                                temp[counter] = 47;
                                counter++;
                                break;
                            }
                        case 58://:
                            {
                                temp[counter] = 58;
                                counter++;
                                break;
                            }
                        case 59://;
                            {
                                temp[counter] = 59;
                                counter++;
                                break;
                            }
                        case 60://<
                            {
                                temp[counter] = 60;
                                counter++;
                                break;
                            }
                        case 61://=
                            {
                                temp[counter] = 61;
                                counter++;
                                break;
                            }
                        case 62://>
                            {
                                temp[counter] = 62;
                                counter++;
                                break;
                            }
                        case 1567://?
                            {
                                temp[counter] = 140;
                                counter++;
                                break;
                            }
                        case 64://@
                            {
                                temp[counter] = 64;
                                counter++;
                                break;
                            }
                        case 91://[
                            {
                                temp[counter] = 91;
                                counter++;
                                break;
                            }
                        case 92://\
                            {
                                temp[counter] = 92;
                                counter++;
                                break;
                            }
                        case 93://]
                            {
                                temp[counter] = 93;
                                counter++;
                                break;
                            }
                        case 94://^
                            {
                                temp[counter] = 94;
                                counter++;
                                break;
                            }
                        case 95://_
                            {
                                temp[counter] = 95;
                                counter++;
                                break;
                            }
                        case 123://{
                            {
                                temp[counter] = 123;
                                counter++;
                                break;
                            }
                        case 124://|
                            {
                                temp[counter] = 124;
                                counter++;
                                break;
                            }
                        case 125://}
                            {
                                temp[counter] = 125;
                                counter++;
                                break;
                            }
                        case 126://~
                            {
                                temp[counter] = 126;
                                counter++;
                                break;
                            }


                        case 48://0
                            {
                                temp[counter] = 128;
                                counter++;
                                break;
                            }
                        case 49://1
                            {
                                temp[counter] = 129;
                                counter++;
                                break;
                            }
                        case 50://2
                            {
                                temp[counter] = 130;
                                counter++;
                                break;
                            }
                        case 51://3
                            {
                                temp[counter] = 131;
                                counter++;
                                break;
                            }
                        case 52://4
                            {
                                temp[counter] = 132;
                                counter++;
                                break;
                            }
                        case 53://5
                            {
                                temp[counter] = 133;
                                counter++;
                                break;
                            }
                        case 54://6
                            {
                                temp[counter] = 134;
                                counter++;
                                break;
                            }
                        case 55://7
                            {
                                temp[counter] = 135;
                                counter++;
                                break;
                            }
                        case 56://8
                            {
                                temp[counter] = 136;
                                counter++;
                                break;
                            }
                        case 57://9
                            {
                                temp[counter] = 137;
                                counter++;
                                break;
                            }
                    }
                    #endregion
                }

                serial.Write(temp, 0, temp.Length);

                temp[counter] = 0x0D;
                counter++;

                temp[counter] = 0x0A;
                counter++;

                Array.Clear(InputArray, 0, InputArray.Length);
                Array.Clear(temp, 0, temp.Length);
            }


        }
        public void PrintEnter()
        {
            serial.Write("\r\n");
        }
        #endregion

        #region PrinterCommandMethods

        public void PrintCut()
        {
            //CTS = serial.CtsHolding;
            byte[] FullCut = { 0x1b, 0x69 }; //FullCut
            serial.Write(FullCut, 0, FullCut.Length);
        }

        public void PrintFontSize_UnderLine(_FontUnderLine select)
        {
            if (select == _FontUnderLine.FontSizeUnderLine)
            {
                byte[] UnderLine = { 0x1B, 0x21, 0xB0 };//UnderLine&FontSize
                serial.Write(UnderLine, 0, UnderLine.Length);
            }
            else if (select == _FontUnderLine.FontSize)
            {
                byte[] FontSize = { 0x1B, 0x21, 0x30 };//FontSize
                serial.Write(FontSize, 0, FontSize.Length);
            }
            else if (select == _FontUnderLine.FontSizeDefault)
            {
                byte[] FontSize = { 0x1B, 0x21, 0x00 };//FontSizeDefault
                serial.Write(FontSize, 0, FontSize.Length);
            }
        }

        public void PrintHorizontalTab()
        {
            //CTS = serial.CtsHolding;
            byte[] HorizontalTab = { 0x09 };//HorizontalTab
            serial.Write(HorizontalTab, 0, HorizontalTab.Length);
        }

        public void PrintUnderLine(_UnderLine select)
        {
            if (select == _UnderLine.OneDotUnderLine)
            {
                byte[] UnderLine = { 0x1B, 0x2D, 0x01 };
                serial.Write(UnderLine, 0, UnderLine.Length);
            }
            else if (select == _UnderLine.TwoDotsUnderLine)
            {
                byte[] UnderLine = { 0x1B, 0x2D, 0x02 };
                serial.Write(UnderLine, 0, UnderLine.Length);
            }

            else if (select == _UnderLine.Reset)
            {
                byte[] UnderLine = { 0x1B, 0x2D };
                serial.Write(UnderLine, 0, UnderLine.Length);
            }

        }

        public _ErrorLists PrintGetStatus()
        {
            _ErrorLists error = _ErrorLists.Normal;

            byte[] Status = { 0x1B, 0x76 };//Status

            serial.ReadExisting();
            serial.Write(Status, 0, Status.Length);

            Thread.Sleep(50);
            byte temp = 0xFF;
            if (serial.BytesToRead > 0)
            {
                temp = (byte)serial.ReadByte();
            }
            #region

            if ((temp & 0x20) == 0x20)
            {
                error = _ErrorLists.PaperDetectError;
            }

            if ((temp & 0x10) == 0x10)
            {
                error = _ErrorLists.AutoCutterError;
            }

            if ((temp & 0x08) == 0x08)
            {
                error = _ErrorLists.PresenterEjectionError;
            }

            if ((temp & 0x04) == 0x04)
            {
                error = _ErrorLists.PaperEnd;
            }

            if ((temp & 0x40) == 0x40)
            {
                error = _ErrorLists.PresenterClamp;
            }

            if ((temp & 0x02) == 0x02)
            {
                error = _ErrorLists.ThermalHeadCoverOpen;
            }

            if ((temp & 0x01) == 0x01)
            {
                error = _ErrorLists.PaperNearEnd;
            }

            if ((temp & 0xFF) == 0xFF)
            {
                error = _ErrorLists.FatalError;
            }
            #endregion
            return error;
        }

        public byte PrintGetStatus_byte()
        {
            byte[] Status = { 0x1B, 0x76 };//Status
            serial.ReadExisting();
            serial.Write(Status, 0, Status.Length);
            Thread.Sleep(50);

            byte temp = 0xFF;
            if (serial.BytesToRead > 0)
            {
                temp = (byte)serial.ReadByte();
            }
            return temp;
        }

        public void PrintBold(_Bold select)
        {
            if (select == _Bold.Bold)
            {
                byte[] Bold = { 0x1B, 0x45, 0x01 };
                serial.Write(Bold, 0, Bold.Length);
            }
            else
            {
                byte[] NotBold = { 0x1B, 0x45, 0x00 };
                serial.Write(NotBold, 0, NotBold.Length);
            }
        }

        public byte PrinterInformation()
        {
            byte[] Info = { 0x1B, 0x73, 0x02 };//Status

            serial.ReadExisting();

            serial.Write(Info, 0, Info.Length);

            System.Threading.Thread.Sleep(50);

            return (byte)serial.ReadByte();
        }

        public void PrinterAutoRetraction()
        {
            byte[] PrinterAutoRetraction = { 0x1B, 0x68, 0x00 };//PrinterAutoRetraction
            serial.Write(PrinterAutoRetraction, 0, PrinterAutoRetraction.Length);
        }

        public void PrinterManualRetraction()
        {
            byte[] PrinterAutoRetraction = { 0x1B, 0x68, 0x02 };//PrinterAutoRetraction
            serial.Write(PrinterAutoRetraction, 0, PrinterAutoRetraction.Length);
        }

        public void Printer_ReInitialize()
        {
            byte[] SoftwareReset = { 0x11 };//SoftwareReset
            for (int i = 0; i < 2; i++)
            {
                serial.Write(SoftwareReset, 0, SoftwareReset.Length);
                Thread.Sleep(2000);
            }

            byte[] PrinterSelect = { 0x1B, 0x3D, 0x01 };//PrinterSelect
            serial.Write(PrinterSelect, 0, PrinterSelect.Length);

            byte[] PrinterInitilization = { 0x1B, 0x40 };//PrinterInitilization
            serial.Write(PrinterInitilization, 0, PrinterInitilization.Length);

            byte[] BackFeed = { 0x1B, 0x42, 0x0F };//In order to avoid paper jam
            serial.Write(BackFeed, 0, BackFeed.Length);

            byte[] PrinterInformation = { 0x1B, 0x73, 0x02 };//PrinterInformation
            serial.Write(PrinterInformation, 0, PrinterInformation.Length);
            temp = serial.ReadExisting();

            byte[] IranSystem = { 0x1B, 0x74, 0x07, 0x0D, 0x0A };//CodePage IranSystem
            serial.Write(IranSystem, 0, IranSystem.Length);

            byte[] PrintDensity = { 0x1D, 0x7E, 0x87 };//PrintDensity
            serial.Write(PrintDensity, 0, PrintDensity.Length);

            byte[] Centering = { 0x1B, 0x61, 0x01 };//Centering
            serial.Write(Centering, 0, Centering.Length);

            byte[] PageBufferClear = { 0x18 };//PageBufferClear
            serial.Write(PageBufferClear, 0, PageBufferClear.Length);

            byte[] ManualEject = { 0x1B, 0x68, 0x02 };//ManualEject
            serial.Write(ManualEject, 0, ManualEject.Length);



        }

        public byte check_keyboard()
        {
            UInt16 length_temp = (UInt16)serial.BytesToRead;
            byte[] temp = new byte[length_temp];

            for (UInt16 k = 0; k < length_temp; k++)
            {
                temp[k] = (byte)serial.ReadByte();
            }

            try
            {
                for (UInt16 k = 0; k < length_temp; k++)
                {
                    if (temp[k] == 0x7E)
                    {
                        if (temp[k + 1] == 0xE7)
                        {
                            if (temp[k + 3] == 0xFF - temp[k + 2])
                            {
                                return temp[k + 2];
                            }
                        }
                    }
                }
            }
            catch { }
            return 0;
        }

        public void go_to_the_nippon_printer()
        {
            IO_Control.Output_Pin(IO_Control.pins.printer_serial_key, IO_Control.pin_status.pin_set);
        }

        public void go_to_the_keyboard()
        {
            IO_Control.Output_Pin(IO_Control.pins.printer_serial_key, IO_Control.pin_status.pin_reset);
        }

        public void clear_for_keyboard()
        {
            serial.ReadExisting();
        }

        #endregion
    }
}
