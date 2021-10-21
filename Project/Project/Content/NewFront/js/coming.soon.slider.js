var tpj=jQuery;

                var revapi152;
                tpj(document).ready(function() {
                    if(tpj("#rev_slider_152_1").revolution == undefined){
                        revslider_showDoubleJqueryError("#rev_slider_152_1");
                    }else{
                        revapi152 = tpj("#rev_slider_152_1").show().revolution({
                            sliderType:"hero",
    jsFileLocation:"revolution/js/",
                            sliderLayout:"fullscreen",
                            dottedOverlay:"none",
                            delay:9000,
                            navigation: {
                            },
                            responsiveLevels:[1240,1024,778,480],
                            visibilityLevels:[1240,1024,778,480],
                            gridwidth:[1240,1024,778,480],
                            gridheight:[868,768,960,720],
                            lazyType:"none",
                            shadow:0,
                            spinner:"off",
                            autoHeight:"off",
                            fullScreenAutoWidth:"off",
                            fullScreenAlignForce:"off",
                            fullScreenOffsetContainer: "",
                            fullScreenOffset: "",
                            disableProgressBar:"on",
                            hideThumbsOnMobile:"off",
                            hideSliderAtLimit:0,
                            hideCaptionAtLimit:0,
                            hideAllCaptionAtLilmit:0,
                            debugMode:false,
                            fallbacks: {
                                simplifyAll:"off",
                                disableFocusListener:false,
                            }
                        });
                    }


                    // SET TARGET DATE TO START COUNT DOWN FROM
                    // SET UNLIMITED TIME STAMPS TO JUMP TO OTHER SLIDES BASED ON THE REST TIME VIA slidechanges
                    // SET THE JUMP AHEAD VIA THE QUICK JUMP  (15000 == 15 sec)
                    // DONT FORGET TO DEFINE THE CONTAINER ID 

                    var targetdate =  new Date().getTime() + 864000000 // i.e. '2015/12/31 24:00',
                        slidechanges = [
                                        { days:0, hours:0, minutes:0, seconds:12, slide:2},
                                        { days:0, hours:0, minutes:0, seconds:0, slide:3}
                                        ];

                     tp_countdown(revapi152,targetdate,slidechanges);       

                    RsTypewriterAddOn(tpj, revapi152);
            });	/*ready*/
                