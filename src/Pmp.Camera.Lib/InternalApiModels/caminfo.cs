﻿namespace Pmp.Camera.Lib.InternalApiModels
{
    //    <? xml version="1.0" encoding="Shift-JIS"?>
    //<oishare>
    //    <version>2.60</version>
    //    <support func = "web" />
    //    < support func="remote"/>
    //    <support func = "gps" />
    //    < support func="release"/>
    //    <cgi name = "get_connectmode" >
    //        < http_method type="get"/>
    //    </cgi>
    //    <cgi name = "switch_cammode" >
    //        < http_method type="get">
    //            <cmd1 name = "mode" >
    //                < param1 name="rec">
    //                    <cmd2 name = "lvqty" >
    //                        < param2 name="0320x0240"/>
    //                        <param2 name = "0640x0480" />
    //                        < param2 name="0800x0600"/>
    //                        <param2 name = "1024x0768" />
    //                        < param2 name="1280x0960"/>
    //                    </cmd2>
    //                </param1>
    //                <param1 name = "play" />
    //                < param1 name="shutter"/>
    //            </cmd1>
    //        </http_method>
    //    </cgi>
    //    <cgi name = "get_caminfo" >
    //        < http_method type="get"/>
    //    </cgi>
    //    <cgi name = "exec_pwoff" >
    //        < http_method type="get"/>
    //    </cgi>
    //    <cgi name = "get_resizeimg" >
    //        < http_method type="get">
    //            <cmd1 name = "DIR" >
    //                < param1 >
    //                    < cmd2 name="size">
    //                        <param2 name = "1024" />
    //                        < param2 name="1600"/>
    //                        <param2 name = "1920" />
    //                        < param2 name="2048"/>
    //                    </cmd2>
    //                </param1>
    //            </cmd1>
    //        </http_method>
    //    </cgi>
    //    <cgi name = "get_movplaytime" >
    //        < http_method type="get">
    //            <cmd1 name = "DIR" />
    //        </ http_method >
    //    </ cgi >
    //    < cgi name="clear_resvflg">
    //        <http_method type = "get" />
    //    </ cgi >
    //    < cgi name="get_rsvimglist">
    //        <http_method type = "get" />
    //    </ cgi >
    //    < cgi name="get_imglist">
    //        <http_method type = "get" >
    //            < cmd1 name="DIR"/>
    //        </http_method>
    //    </cgi>
    //    <cgi name = "get_thumbnail" >
    //        < http_method type="get">
    //            <cmd1 name = "DIR" />
    //        </ http_method >
    //    </ cgi >
    //    < cgi name="get_screennail">
    //        <http_method type = "get" >
    //            < cmd1 name="DIR"/>
    //        </http_method>
    //    </cgi>
    //    <cgi name = "exec_takemotion" >
    //        < http_method type="get">
    //            <cmd1 name = "com" >
    //                < param1 name="assignafframe">
    //                    <cmd2 name = "point" />
    //                </ param1 >
    //                < param1 name="releaseafframe"/>
    //                <param1 name = "takeready" >
    //                    < cmd2 name="point"/>
    //                </param1>
    //                <param1 name = "starttake" >
    //                    < cmd2 name="point">
    //                        <cmd3 name = "exposuremin" />
    //                        < cmd3 name="upperlimit"/>
    //                    </cmd2>
    //                </param1>
    //                <param1 name = "stoptake" />
    //                < param1 name="startmovietake">
    //                    <cmd2 name = "limitter" />
    //                    < cmd3 name="liveview">
    //                        <param3 name = "on" />
    //                    </ cmd3 >
    //                </ param1 >
    //                < param1 name="stopmovietake"/>
    //            </cmd1>
    //        </http_method>
    //    </cgi>
    //    <cgi name = "exec_takemisc" >
    //        < http_method type="get">
    //            <cmd1 name = "com" >
    //                < param1 name="startliveview">
    //                    <cmd2 name = "port" />
    //                </ param1 >
    //                < param1 name="stopliveview"/>
    //                <param1 name = "getrecview" />
    //                < param1 name="getlastjpg"/>
    //                <param1 name = "ctrlzoom" >
    //                    < cmd2 name="move">
    //                        <param2 name = "widemove" />
    //                        < param2 name="telemove"/>
    //                        <param2 name = "off" />
    //                        < param2 name="wideterm"/>
    //                        <param2 name = "teleterm" />
    //                    </ cmd2 >
    //                </ param1 >
    //                < param1 name="GetShortMoviesAlbumInfo"/>
    //            </cmd1>
    //        </http_method>
    //    </cgi>
    //    <cgi name = "get_camprop" >
    //        < http_method type="get">
    //            <cmd1 name = "com" >
    //                < param1 name="desc">
    //                    <cmd2 name = "propname" >
    //                        < param2 name="touchactiveframe"/>
    //                        <param2 name = "takemode" />
    //                        < param2 name="drivemode"/>
    //                        <param2 name = "focalvalue" />
    //                        < param2 name="expcomp"/>
    //                        <param2 name = "shutspeedvalue" />
    //                        < param2 name="isospeedvalue"/>
    //                        <param2 name = "wbvalue" />
    //                        < param2 name="noisereduction"/>
    //                        <param2 name = "lowvibtime" />
    //                        < param2 name="bulbtimelimit"/>
    //                        <param2 name = "artfilter" />
    //                        < param2 name="digitaltelecon"/>
    //                        <param2 name = "cameradrivemode" />
    //                        < param2 name="colortone"/>
    //                        <param2 name = "exposemovie" />
    //                        < param2 name="colorphase"/>
    //                        <param2 name = "QualityMovie2" />
    //                        < param2 name="PixelShort"/>
    //                        <param2 name = "PixelSet1" />
    //                        < param2 name="PixelSet2"/>
    //                        <param2 name = "PixelSet3" />
    //                        < param2 name="PixelSet4"/>
    //                        <param2 name = "PixelCustom" />
    //                        < param2 name="CompShort"/>
    //                        <param2 name = "CompSet1" />
    //                        < param2 name="CompSet2"/>
    //                        <param2 name = "CompSet3" />
    //                        < param2 name="CompSet4"/>
    //                        <param2 name = "CompCustom" />
    //                        < param2 name="FrameRateShort"/>
    //                        <param2 name = "FrameRateSet1" />
    //                        < param2 name="FrameRateSet2"/>
    //                        <param2 name = "FrameRateSet3" />
    //                        < param2 name="FrameRateSet4"/>
    //                        <param2 name = "FrameRateCustom" />
    //                        < param2 name="RecordTimeShort"/>
    //                        <param2 name = "RecordTimeCustom" />
    //                        < param2 name="NoiseReductionExposureTime"/>
    //                        <param2 name = "SilentNoiseReduction" />
    //                        < param2 name="SilentTime"/>
    //                        <param2 name = "desclist" />
    //                    </ cmd2 >
    //                </ param1 >
    //                < param1 name="get">
    //                    <cmd2 name = "propname" >
    //                        < param2 name="touchactiveframe"/>
    //                        <param2 name = "takemode" />
    //                        < param2 name="drivemode"/>
    //                        <param2 name = "focalvalue" />
    //                        < param2 name="expcomp"/>
    //                        <param2 name = "shutspeedvalue" />
    //                        < param2 name="isospeedvalue"/>
    //                        <param2 name = "wbvalue" />
    //                        < param2 name="noisereduction"/>
    //                        <param2 name = "lowvibtime" />
    //                        < param2 name="bulbtimelimit"/>
    //                        <param2 name = "artfilter" />
    //                        < param2 name="digitaltelecon"/>
    //                        <param2 name = "cameradrivemode" />
    //                        < param2 name="colortone"/>
    //                        <param2 name = "exposemovie" />
    //                        < param2 name="colorphase"/>
    //                        <param2 name = "QualityMovie2" />
    //                        < param2 name="PixelShort"/>
    //                        <param2 name = "PixelSet1" />
    //                        < param2 name="PixelSet2"/>
    //                        <param2 name = "PixelSet3" />
    //                        < param2 name="PixelSet4"/>
    //                        <param2 name = "PixelCustom" />
    //                        < param2 name="CompShort"/>
    //                        <param2 name = "CompSet1" />
    //                        < param2 name="CompSet2"/>
    //                        <param2 name = "CompSet3" />
    //                        < param2 name="CompSet4"/>
    //                        <param2 name = "CompCustom" />
    //                        < param2 name="FrameRateShort"/>
    //                        <param2 name = "FrameRateSet1" />
    //                        < param2 name="FrameRateSet2"/>
    //                        <param2 name = "FrameRateSet3" />
    //                        < param2 name="FrameRateSet4"/>
    //                        <param2 name = "FrameRateCustom" />
    //                        < param2 name="RecordTimeShort"/>
    //                        <param2 name = "RecordTimeCustom" />
    //                        < param2 name="NoiseReductionExposureTime"/>
    //                        <param2 name = "SilentNoiseReduction" />
    //                        < param2 name="SilentTime"/>
    //                    </cmd2>
    //                </param1>
    //                <param1 name = "check" >
    //                    < cmd2 name="propname">
    //                        <param2 name = "CompShort" />
    //                        < param2 name="CompSet1"/>
    //                        <param2 name = "CompSet2" />
    //                        < param2 name="CompSet3"/>
    //                        <param2 name = "CompSet4" />
    //                        < param2 name="CompCustom"/>
    //                        <param2 name = "FrameRateShort" />
    //                        < param2 name="FrameRateSet1"/>
    //                        <param2 name = "FrameRateSet2" />
    //                        < param2 name="FrameRateSet3"/>
    //                        <param2 name = "FrameRateSet4" />
    //                        < param2 name="FrameRateCustom"/>
    //                    </cmd2>
    //                </param1>
    //            </cmd1>
    //        </http_method>
    //    </cgi>
    //    <cgi name = "set_camprop" >
    //        < http_method type="post">
    //            <cmd1 name = "com" >
    //                < param1 name="set">
    //                    <cmd2 name = "propname" >
    //                        < param2 name="takemode"/>
    //                        <param2 name = "drivemode" />
    //                        < param2 name="focalvalue"/>
    //                        <param2 name = "expcomp" />
    //                        < param2 name="shutspeedvalue"/>
    //                        <param2 name = "isospeedvalue" />
    //                        < param2 name="wbvalue"/>
    //                        <param2 name = "artfilter" />
    //                        < param2 name="colortone"/>
    //                        <param2 name = "exposemovie" />
    //                        < param2 name="colorphase"/>
    //                    </cmd2>
    //                </param1>
    //            </cmd1>
    //        </http_method>
    //    </cgi>
    //    <cgi name = "get_activate" >
    //        < http_method type="get"/>
    //    </cgi>
    //    <cgi name = "set_utctimediff" >
    //        < http_method type="get">
    //            <cmd1 name = "utctime" >
    //                < cmd2 name="diff"/>
    //            </cmd1>
    //        </http_method>
    //    </cgi>
    //    <cgi name = "get_gpsdivunit" >
    //        < http_method type="get"/>
    //    </cgi>
    //    <cgi name = "get_unusedcapacity" >
    //        < http_method type="get"/>
    //    </cgi>
    //    <cgi name = "get_dcffilenum" >
    //        < http_method type="get"/>
    //    </cgi>
    //    <cgi name = "req_attachexifgps" >
    //        < http_method type="post"/>
    //    </cgi>
    //    <cgi name = "req_storegpsinfo" >
    //        < http_method type="post">
    //            <cmd1 name = "mode" >
    //                < param1 name="new"/>
    //                <param1 name = "append" />
    //                < cmd2 name="date"/>
    //            </cmd1>
    //        </http_method>
    //    </cgi>
    //    <cgi name = "exec_shutter" >
    //        < http_method type="get">
    //            <cmd1 name = "com" >
    //                < param1 name="1stpush"/>
    //                <param1 name = "2ndpush" />
    //                < param1 name="1st2ndpush"/>
    //                <param1 name = "2nd1strelease" />
    //                < param1 name="2ndrelease"/>
    //                <param1 name = "1strelease" />
    //            </ cmd1 >
    //        </ http_method >
    //    </ cgi >
    //</ oishare >
    

    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class caminfo
    {

        private string modelField;

        /// <remarks/>0
        public string model
        {
            get
            {
                return this.modelField;
            }
            set
            {
                this.modelField = value;
            }
        }
    }
}
