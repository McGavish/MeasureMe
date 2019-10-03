using System;
using System.IO;
using System.Collections.Generic;
using System.Xml.Serialization;

public class CommandsResponseReader
{
    [XmlRoot(ElementName = "desc")]
    public class Desc
    {
        [XmlElement(ElementName = "propname")]
        public string Propname { get; set; }
        [XmlElement(ElementName = "attribute")]
        public string Attribute { get; set; }
        [XmlElement(ElementName = "value")]
        public string Value { get; set; }
        [XmlElement(ElementName = "enum")]
        public string Enum { get; set; }
    }

    [XmlRoot(ElementName = "desclist")]
    public class Desclist
    {
        [XmlElement(ElementName = "desc")]
        public List<Desc> Desc { get; set; }
    }

    [XmlRoot(ElementName = "get")]
    public class Get
    {
        [XmlElement(ElementName = "value")]
        public string Value { get; set; }
    }

    public static string TryGetValue(string response)
    {
        var result = response;
        try
        {
            var serializer = new XmlSerializer(typeof(Get));
            var tr = new StringReader(response);
            var obj = (Get)serializer.Deserialize(tr);
            result = obj.Value;
        }
        catch (Exception)
        {
        }
        return result;
    }

    public static Desclist GetDescList()
    {
        var desc = "<?xml version=\"1.0\"?><desclist><desc><propname>shutspeedvalue</propname><attribute>getset</attribute><value>250</value><enum>livecomp livetime livebulb 60\" 50\" 40\" 30\" 25\" 20\" 15\" 13\" 10\" 8\" 6\" 5\" 4\" 3.2\" 2.5\" 2\" 1.6\" 1.3\" 1\" 1.3 1.6 2 2.5 3 4 5 6 8 10 13 15 20 25 30 40 50 60 80 100 125 160 200 250 320 400 500 640 800 1000 1250 1600 2000 2500 3200 4000</enum></desc><desc><propname>touchactiveframe</propname><attribute>get</attribute><value>0064-0048_0192x0144</value></desc><desc><propname>takemode</propname><attribute>getset</attribute><value>P</value><enum>iAuto P A S M ART movie</enum></desc><desc><propname>lowvibtime</propname><attribute>get</attribute><value>0</value><enum>0 125 250 500 1000 2000 4000 8000 15000 30000 -1</enum></desc><desc><propname>cameradrivemode</propname><attribute>get</attribute><value>normal</value><enum>normal normal normal continuous continuous continuous continuous continuous selftimer selftimer selftimer customselftimer customselftimer customselftimer</enum></desc><desc><propname>SilentTime</propname><attribute>get</attribute><value>0</value><enum>0 125 250 500 1000 2000 4000 8000 15000 30000 -1</enum></desc><desc><propname>drivemode</propname><attribute>getset</attribute><value>normal</value><enum>normal lowvib-normal silent-normal continuous-H silent-continuous-H continuous-L lowvib-continuous-L silent-continuous-L selftimer lowvib-selftimer silent-selftimer customselftimer lowvib-customselftimer silent-customselftimer</enum></desc><desc><propname>focalvalue</propname><attribute>getset</attribute><value>5.6</value><enum>1.0 1.1 1.2 1.4 1.6 1.8 2.0 2.2 2.5 2.8 3.2 3.5 4.0 4.5 5.0 5.6 6.3 7.1 8.0 9.0 10 11 13 14 16 18 20 22 25 29 32 36 40 45 51 57 64 72 81 91</enum></desc><desc><propname>expcomp</propname><attribute>getset</attribute><value>+0.3</value><enum>-5.0 -4.7 -4.3 -4.0 -3.7 -3.3 -3.0 -2.7 -2.3 -2.0 -1.7 -1.3 -1.0 -0.7 -0.3 0.0 +0.3 +0.7 +1.0 +1.3 +1.7 +2.0 +2.3 +2.7 +3.0 +3.3 +3.7 +4.0 +4.3 +4.7 +5.0</enum></desc><desc><propname>isospeedvalue</propname><attribute>getset</attribute><value>200</value><enum>Auto Low 200 250 320 400 500 640 800 1000 1250 1600 2000 2500 3200 4000 5000 6400 8000 10000 12800 16000 20000 25600</enum></desc><desc><propname>wbvalue</propname><attribute>getset</attribute><value>0</value><enum>0 18 16 17 20 35 64 23 256 257 258 259 512</enum></desc><desc><propname>artfilter</propname><attribute>getset</attribute><value>light_tone</value><enum>popart fantasic_focus daydream light_tone rough_monochrome toy_photo miniature cross_process gentle_sepia dramatic_tone ligne_clair pastel vintage partcolor program</enum></desc><desc><propname>colortone</propname><attribute>getset</attribute><value>flat</value><enum>ifinish vivid natural flat portrait monotone custom eportrait coloacreator popart fantasic_focus daydream light_tone rough_monochrome toy_photo miniature cross_process gentle_sepia dramatic_tone ligne_clair pastel vintage partcolor</enum></desc><desc><propname>exposemovie</propname><attribute>getset</attribute><value>P</value><enum>P A S M</enum></desc><desc><propname>colorphase</propname><attribute>getset</attribute><value>step1</value><enum>step0 step1 step2 step3 step4 step5 step6 step7 step8 step9 step10 step11 step12 step13 step14 step15 step16 step17</enum></desc><desc><propname>QualityMovie2</propname><attribute>get</attribute><value>set1</value><enum>shortmovie set1 set2 set3 set4 custom</enum></desc><desc><propname>PixelShort</propname><attribute>get</attribute><value>1080p</value><enum>1080p</enum></desc><desc><propname>PixelSet1</propname><attribute>get</attribute><value>1080p</value><enum>1080p 720p</enum></desc><desc><propname>PixelSet2</propname><attribute>get</attribute><value>1080p</value><enum>1080p 720p</enum></desc><desc><propname>PixelSet3</propname><attribute>get</attribute><value>1080p</value><enum>1080p 720p</enum></desc><desc><propname>PixelSet4</propname><attribute>get</attribute><value>1080p</value><enum>1080p 720p</enum></desc><desc><propname>PixelCustom</propname><attribute>get</attribute><value>1080p</value><enum>1080p</enum></desc><desc><propname>CompShort</propname><attribute>get</attribute><value>fine</value><enum>fine</enum></desc><desc><propname>CompSet1</propname><attribute>get</attribute><value>fine</value><enum>fine normal</enum></desc><desc><propname>CompSet2</propname><attribute>get</attribute><value>fine</value><enum>fine normal</enum></desc><desc><propname>CompSet3</propname><attribute>get</attribute><value>fine</value><enum>fine normal</enum></desc><desc><propname>CompSet4</propname><attribute>get</attribute><value>normal</value><enum>fine normal</enum></desc><desc><propname>CompCustom</propname><attribute>get</attribute><value>fine</value><enum>fine normal</enum></desc><desc><propname>FrameRateShort</propname><attribute>get</attribute><value>25p</value><enum>24p 25p 30p</enum></desc><desc><propname>FrameRateSet1</propname><attribute>get</attribute><value>25p</value><enum>24p 25p 30p</enum></desc><desc><propname>FrameRateSet2</propname><attribute>get</attribute><value>25p</value><enum>24p 25p 30p</enum></desc><desc><propname>FrameRateSet3</propname><attribute>get</attribute><value>25p</value><enum>24p 25p 30p</enum></desc><desc><propname>FrameRateSet4</propname><attribute>get</attribute><value>25p</value><enum>24p 25p 30p</enum></desc><desc><propname>FrameRateCustom</propname><attribute>get</attribute><value>25p</value><enum>24p 25p 30p</enum></desc><desc><propname>RecordTimeShort</propname><attribute>get</attribute><value>4s</value><enum>1s 2s 4s 8s</enum></desc><desc><propname>RecordTimeCustom</propname><attribute>get</attribute><value>off</value><enum>1s 2s 4s 8s off</enum></desc><desc><propname>NoiseReductionExposureTime</propname><attribute>get</attribute><value>1\"</value><enum>60\" 50\" 40\" 30\" 25\" 20\" 15\" 13\" 10\" 8\" 6\" 5\" 4\" 3.2\" 2.5\" 2\" 1.6\" 1.3\" 1\" 1.3 1.6 2</enum></desc><desc><propname>SilentNoiseReduction</propname><attribute>get</attribute><value>off</value><enum>off auto</enum></desc><desc><propname>noisereduction</propname><attribute>get</attribute><value>auto</value><enum>off on auto</enum></desc><desc><propname>bulbtimelimit</propname><attribute>get</attribute><value>8</value><enum>1 2 4 8 15 20 25 30</enum></desc><desc><propname>digitaltelecon</propname><attribute>get</attribute><value>off</value><enum>off on</enum></desc></desclist>";
        return GetDescList(desc);
    }

    public static Desclist GetDescList(string desc)
    {
        var serializer = new XmlSerializer(typeof(Desclist));
        var tr = new StringReader(desc);
        var obj = (Desclist)serializer.Deserialize(tr);
        return obj;
    }

    public static Oishare Read()
    {
        var xml = "<oishare>\r\n    <version>2.60</version>\r\n    <support func=\"web\" />\r\n    <support func=\"remote\"/>\r\n    <support func=\"gps\" />\r\n    <support func=\"release\"/>\r\n    <cgi name=\"get_connectmode\" >\r\n        <http_method type=\"get\"/>\r\n    </cgi>\r\n    <cgi name=\"switch_cammode\" >\r\n        <http_method type=\"get\">\r\n            <cmd1 name=\"mode\" >\r\n                <param1 name=\"rec\">\r\n                    <cmd2 name=\"lvqty\" >\r\n                        <param2 name=\"0320x0240\"/>\r\n                        <param2 name=\"0640x0480\" />\r\n                        <param2 name=\"0800x0600\"/>\r\n                        <param2 name=\"1024x0768\" />\r\n                        <param2 name=\"1280x0960\"/>\r\n                    </cmd2>\r\n                </param1>\r\n                <param1 name=\"play\" />\r\n                <param1 name=\"shutter\"/>\r\n            </cmd1>\r\n        </http_method>\r\n    </cgi>\r\n    <cgi name=\"get_caminfo\" >\r\n        <http_method type=\"get\"/>\r\n    </cgi>\r\n    <cgi name=\"exec_pwoff\" >\r\n        <http_method type=\"get\"/>\r\n    </cgi>\r\n    <cgi name=\"get_resizeimg\" >\r\n        <http_method type=\"get\">\r\n            <cmd1 name=\"DIR\" >\r\n                <param1 >\r\n                    <cmd2 name=\"size\">\r\n                        <param2 name=\"1024\" />\r\n                        <param2 name=\"1600\"/>\r\n                        <param2 name=\"1920\" />\r\n                        <param2 name=\"2048\"/>\r\n                    </cmd2>\r\n                </param1>\r\n            </cmd1>\r\n        </http_method>\r\n    </cgi>\r\n    <cgi name=\"get_movplaytime\" >\r\n        <http_method type=\"get\">\r\n            <cmd1 name=\"DIR\" />\r\n        </http_method >\r\n    </cgi >\r\n    <cgi name=\"clear_resvflg\">\r\n        <http_method type=\"get\" />\r\n    </cgi >\r\n    <cgi name=\"get_rsvimglist\">\r\n        <http_method type=\"get\" />\r\n    </cgi >\r\n    <cgi name=\"get_imglist\">\r\n        <http_method type=\"get\" >\r\n            <cmd1 name=\"DIR\"/>\r\n        </http_method>\r\n    </cgi>\r\n    <cgi name=\"get_thumbnail\" >\r\n        <http_method type=\"get\">\r\n            <cmd1 name=\"DIR\" />\r\n        </http_method >\r\n    </cgi >\r\n    <cgi name=\"get_screennail\">\r\n        <http_method type=\"get\" >\r\n            <cmd1 name=\"DIR\"/>\r\n        </http_method>\r\n    </cgi>\r\n    <cgi name=\"exec_takemotion\" >\r\n        <http_method type=\"get\">\r\n            <cmd1 name=\"com\" >\r\n                <param1 name=\"assignafframe\">\r\n                    <cmd2 name=\"point\" />\r\n                </param1 >\r\n                <param1 name=\"releaseafframe\"/>\r\n                <param1 name=\"takeready\" >\r\n                    <cmd2 name=\"point\"/>\r\n                </param1>\r\n                <param1 name=\"starttake\" >\r\n                    <cmd2 name=\"point\">\r\n                        <cmd3 name=\"exposuremin\" />\r\n                        <cmd3 name=\"upperlimit\"/>\r\n                    </cmd2>\r\n                </param1>\r\n                <param1 name=\"stoptake\" />\r\n                <param1 name=\"startmovietake\">\r\n                    <cmd2 name=\"limitter\" />\r\n                    <cmd3 name=\"liveview\">\r\n                        <param3 name=\"on\" />\r\n                    </cmd3 >\r\n                </param1 >\r\n                <param1 name=\"stopmovietake\"/>\r\n            </cmd1>\r\n        </http_method>\r\n    </cgi>\r\n    <cgi name=\"exec_takemisc\" >\r\n        <http_method type=\"get\">\r\n            <cmd1 name=\"com\" >\r\n                <param1 name=\"startliveview\">\r\n                    <cmd2 name=\"port\" />\r\n                </param1 >\r\n                <param1 name=\"stopliveview\"/>\r\n                <param1 name=\"getrecview\" />\r\n                <param1 name=\"getlastjpg\"/>\r\n                <param1 name=\"ctrlzoom\" >\r\n                    <cmd2 name=\"move\">\r\n                        <param2 name=\"widemove\" />\r\n                        <param2 name=\"telemove\"/>\r\n                        <param2 name=\"off\" />\r\n                        <param2 name=\"wideterm\"/>\r\n                        <param2 name=\"teleterm\" />\r\n                    </cmd2 >\r\n                </param1 >\r\n                <param1 name=\"GetShortMoviesAlbumInfo\"/>\r\n            </cmd1>\r\n        </http_method>\r\n    </cgi>\r\n    <cgi name=\"get_camprop\" >\r\n        <http_method type=\"get\">\r\n            <cmd1 name=\"com\" >\r\n                <param1 name=\"desc\">\r\n                    <cmd2 name=\"propname\" >\r\n                        <param2 name=\"touchactiveframe\"/>\r\n                        <param2 name=\"takemode\" />\r\n                        <param2 name=\"drivemode\"/>\r\n                        <param2 name=\"focalvalue\" />\r\n                        <param2 name=\"expcomp\"/>\r\n                        <param2 name=\"shutspeedvalue\" />\r\n                        <param2 name=\"isospeedvalue\"/>\r\n                        <param2 name=\"wbvalue\" />\r\n                        <param2 name=\"noisereduction\"/>\r\n                        <param2 name=\"lowvibtime\" />\r\n                        <param2 name=\"bulbtimelimit\"/>\r\n                        <param2 name=\"artfilter\" />\r\n                        <param2 name=\"digitaltelecon\"/>\r\n                        <param2 name=\"cameradrivemode\" />\r\n                        <param2 name=\"colortone\"/>\r\n                        <param2 name=\"exposemovie\" />\r\n                        <param2 name=\"colorphase\"/>\r\n                        <param2 name=\"QualityMovie2\" />\r\n                        <param2 name=\"PixelShort\"/>\r\n                        <param2 name=\"PixelSet1\" />\r\n                        <param2 name=\"PixelSet2\"/>\r\n                        <param2 name=\"PixelSet3\" />\r\n                        <param2 name=\"PixelSet4\"/>\r\n                        <param2 name=\"PixelCustom\" />\r\n                        <param2 name=\"CompShort\"/>\r\n                        <param2 name=\"CompSet1\" />\r\n                        <param2 name=\"CompSet2\"/>\r\n                        <param2 name=\"CompSet3\" />\r\n                        <param2 name=\"CompSet4\"/>\r\n                        <param2 name=\"CompCustom\" />\r\n                        <param2 name=\"FrameRateShort\"/>\r\n                        <param2 name=\"FrameRateSet1\" />\r\n                        <param2 name=\"FrameRateSet2\"/>\r\n                        <param2 name=\"FrameRateSet3\" />\r\n                        <param2 name=\"FrameRateSet4\"/>\r\n                        <param2 name=\"FrameRateCustom\" />\r\n                        <param2 name=\"RecordTimeShort\"/>\r\n                        <param2 name=\"RecordTimeCustom\" />\r\n                        <param2 name=\"NoiseReductionExposureTime\"/>\r\n                        <param2 name=\"SilentNoiseReduction\" />\r\n                        <param2 name=\"SilentTime\"/>\r\n                        <param2 name=\"desclist\" />\r\n                    </cmd2 >\r\n                </param1>\r\n                <param1 name=\"get\">\r\n                    <cmd2 name=\"propname\" >\r\n                        <param2 name=\"touchactiveframe\"/>\r\n                        <param2 name=\"takemode\" />\r\n                        <param2 name=\"drivemode\"/>\r\n                        <param2 name=\"focalvalue\" />\r\n                        <param2 name=\"expcomp\"/>\r\n                        <param2 name=\"shutspeedvalue\" />\r\n                        <param2 name=\"isospeedvalue\"/>\r\n                        <param2 name=\"wbvalue\" />\r\n                        <param2 name=\"noisereduction\"/>\r\n                        <param2 name=\"lowvibtime\" />\r\n                        <param2 name=\"bulbtimelimit\"/>\r\n                        <param2 name=\"artfilter\" />\r\n                        <param2 name=\"digitaltelecon\"/>\r\n                        <param2 name=\"cameradrivemode\" />\r\n                        <param2 name=\"colortone\"/>\r\n                        <param2 name=\"exposemovie\" />\r\n                        <param2 name=\"colorphase\"/>\r\n                        <param2 name=\"QualityMovie2\" />\r\n                        <param2 name=\"PixelShort\"/>\r\n                        <param2 name=\"PixelSet1\" />\r\n                        <param2 name=\"PixelSet2\"/>\r\n                        <param2 name=\"PixelSet3\" />\r\n                        <param2 name=\"PixelSet4\"/>\r\n                        <param2 name=\"PixelCustom\" />\r\n                        <param2 name=\"CompShort\"/>\r\n                        <param2 name=\"CompSet1\" />\r\n                        <param2 name=\"CompSet2\"/>\r\n                        <param2 name=\"CompSet3\" />\r\n                        <param2 name=\"CompSet4\"/>\r\n                        <param2 name=\"CompCustom\" />\r\n                        <param2 name=\"FrameRateShort\"/>\r\n                        <param2 name=\"FrameRateSet1\" />\r\n                        <param2 name=\"FrameRateSet2\"/>\r\n                        <param2 name=\"FrameRateSet3\" />\r\n                        <param2 name=\"FrameRateSet4\"/>\r\n                        <param2 name=\"FrameRateCustom\" />\r\n                        <param2 name=\"RecordTimeShort\"/>\r\n                        <param2 name=\"RecordTimeCustom\" />\r\n                        <param2 name=\"NoiseReductionExposureTime\"/>\r\n                        <param2 name=\"SilentNoiseReduction\" />\r\n                        <param2 name=\"SilentTime\"/>\r\n                    </cmd2>\r\n                </param1>\r\n                <param1 name=\"check\" >\r\n                    <cmd2 name=\"propname\">\r\n                        <param2 name=\"CompShort\" />\r\n                        <param2 name=\"CompSet1\"/>\r\n                        <param2 name=\"CompSet2\" />\r\n                        <param2 name=\"CompSet3\"/>\r\n                        <param2 name=\"CompSet4\" />\r\n                        <param2 name=\"CompCustom\"/>\r\n                        <param2 name=\"FrameRateShort\" />\r\n                        <param2 name=\"FrameRateSet1\"/>\r\n                        <param2 name=\"FrameRateSet2\" />\r\n                        <param2 name=\"FrameRateSet3\"/>\r\n                        <param2 name=\"FrameRateSet4\" />\r\n                        <param2 name=\"FrameRateCustom\"/>\r\n                    </cmd2>\r\n                </param1>\r\n            </cmd1>\r\n        </http_method>\r\n    </cgi>\r\n    <cgi name=\"set_camprop\" >\r\n        <http_method type=\"post\">\r\n            <cmd1 name=\"com\" >\r\n                <param1 name=\"set\">\r\n                    <cmd2 name=\"propname\" >\r\n                        <param2 name=\"takemode\"/>\r\n                        <param2 name=\"drivemode\" />\r\n                        <param2 name=\"focalvalue\"/>\r\n                        <param2 name=\"expcomp\" />\r\n                        <param2 name=\"shutspeedvalue\"/>\r\n                        <param2 name=\"isospeedvalue\" />\r\n                        <param2 name=\"wbvalue\"/>\r\n                        <param2 name=\"artfilter\" />\r\n                        <param2 name=\"colortone\"/>\r\n                        <param2 name=\"exposemovie\" />\r\n                        <param2 name=\"colorphase\"/>\r\n                    </cmd2>\r\n                </param1>\r\n            </cmd1>\r\n        </http_method>\r\n    </cgi>\r\n    <cgi name=\"get_activate\" >\r\n        <http_method type=\"get\"/>\r\n    </cgi>\r\n    <cgi name=\"set_utctimediff\" >\r\n        <http_method type=\"get\">\r\n            <cmd1 name=\"utctime\" >\r\n                <cmd2 name=\"diff\"/>\r\n            </cmd1>\r\n        </http_method>\r\n    </cgi>\r\n    <cgi name=\"get_gpsdivunit\" >\r\n        <http_method type=\"get\"/>\r\n    </cgi>\r\n    <cgi name=\"get_unusedcapacity\" >\r\n        <http_method type=\"get\"/>\r\n    </cgi>\r\n    <cgi name=\"get_dcffilenum\" >\r\n        <http_method type=\"get\"/>\r\n    </cgi>\r\n    <cgi name=\"req_attachexifgps\" >\r\n        <http_method type=\"post\"/>\r\n    </cgi>\r\n    <cgi name=\"req_storegpsinfo\" >\r\n        <http_method type=\"post\">\r\n            <cmd1 name=\"mode\" >\r\n                <param1 name=\"new\"/>\r\n                <param1 name=\"append\" />\r\n                <cmd2 name=\"date\"/>\r\n            </cmd1>\r\n        </http_method>\r\n    </cgi>\r\n    <cgi name=\"exec_shutter\" >\r\n        <http_method type=\"get\">\r\n            <cmd1 name=\"com\" >\r\n                <param1 name=\"1stpush\"/>\r\n                <param1 name=\"2ndpush\" />\r\n                <param1 name=\"1st2ndpush\"/>\r\n                <param1 name=\"2nd1strelease\" />\r\n                <param1 name=\"2ndrelease\"/>\r\n                <param1 name=\"1strelease\" />\r\n            </cmd1 >\r\n        </http_method>\r\n    </cgi>\r\n</oishare>";
        var serializer = new XmlSerializer(typeof(Oishare));
        var tr = new StringReader(xml);
        var obj  = (Oishare)serializer.Deserialize(tr);
        return obj;
    }

    [XmlRoot(ElementName = "support")]
    public class Support
    {
        [XmlAttribute(AttributeName = "func")]
        public string Func { get; set; }
    }

    [XmlRoot(ElementName = "http_method")]
    public class Http_method
    {
        [XmlAttribute(AttributeName = "type")]
        public string Type { get; set; }
        [XmlElement(ElementName = "cmd1")]
        public Cmd1 Cmd1 { get; set; }
    }

    [XmlRoot(ElementName = "cgi")]
    public class Cgi
    {
        [XmlElement(ElementName = "http_method")]
        public Http_method Http_method { get; set; }
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
    }

    [XmlRoot(ElementName = "param2")]
    public class Param2
    {
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
    }

    [XmlRoot(ElementName = "cmd2")]
    public class Cmd2
    {
        [XmlElement(ElementName = "param2")]
        public List<Param2> Param2 { get; set; }
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "cmd3")]
        public List<Cmd3> Cmd3 { get; set; }
    }

    [XmlRoot(ElementName = "param1")]
    public class Param1
    {
        [XmlElement(ElementName = "cmd2")]
        public Cmd2 Cmd2 { get; set; }
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "cmd3")]
        public Cmd3 Cmd3 { get; set; }
    }

    [XmlRoot(ElementName = "cmd1")]
    public class Cmd1
    {
        [XmlElement(ElementName = "param1")]
        public List<Param1> Param1 { get; set; }
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "cmd2")]
        public Cmd2 Cmd2 { get; set; }
    }

    [XmlRoot(ElementName = "cmd3")]
    public class Cmd3
    {
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "param3")]
        public Param3 Param3 { get; set; }
    }

    [XmlRoot(ElementName = "param3")]
    public class Param3
    {
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
    }

    [XmlRoot(ElementName = "oishare")]
    public class Oishare
    {
        [XmlElement(ElementName = "version")]
        public string Version { get; set; }
        [XmlElement(ElementName = "support")]
        public List<Support> Support { get; set; }
        [XmlElement(ElementName = "cgi")]
        public List<Cgi> Cgi { get; set; }
    }
}
