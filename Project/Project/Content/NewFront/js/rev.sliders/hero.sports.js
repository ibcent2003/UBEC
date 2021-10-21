var tpj=jQuery;
			
			var revapi497;
			tpj(document).ready(function() {
				if(tpj("#rev_slider_497_1").revolution == undefined){
					revslider_showDoubleJqueryError("#rev_slider_497_1");
				}else{
					revapi497 = tpj("#rev_slider_497_1").show().revolution({
						sliderType:"hero",
jsFileLocation:"revolution/js/",
						sliderLayout:"fullwidth",
						dottedOverlay:"none",
						delay:9000,
						navigation: {
						},
						responsiveLevels:[1240,1024,778,480],
						visibilityLevels:[1240,1024,778,480],
						gridwidth:[1240,1024,778,480],
						gridheight:[720,640,640,640],
						lazyType:"none",
						parallax: {
							type:"scroll",
							origo:"enterpoint",
							speed:400,
							levels:[5,10,15,20,25,30,35,40,45,50,46,47,48,49,50,55],
							type:"scroll",
						},
						shadow:0,
						spinner:"off",
						autoHeight:"off",
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
			});	/*ready*/