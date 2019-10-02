using System;
using System.IO;
using System.Collections.Generic;
using System.Xml.Serialization;

public class CommandsResponseReader
{
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
