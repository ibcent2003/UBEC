var revapi314,
	tpj=jQuery;
			
tpj(document).ready(function() {
	if(tpj("#rev_slider_314_1").revolution == undefined){
		revslider_showDoubleJqueryError("#rev_slider_314_1");
	}else{
		revapi314 = tpj("#rev_slider_314_1").show().revolution({
			sliderType:"hero",
			jsFileLocation:"//server.local/revslider/wp-content/plugins/revslider/public/assets/js/",
			sliderLayout:"fullscreen",
			dottedOverlay:"none",
			delay:9000,
			responsiveLevels:[1240,1024,778,480],
			visibilityLevels:[1240,1024,778,480],
			gridwidth:[1240,1024,778,480],
			gridheight:[868,768,960,720],
			lazyType:"none",
			parallax: {
				type:"scroll",
				origo:"slidercenter",
				speed:400,
				levels:[1,2,3,4,5,6,7,8,9,10,15,48,49,50,51,55],
			},
			shadow:0,
			spinner:"spinner3",
			autoHeight:"off",
			fullScreenAutoWidth:"off",
			fullScreenAlignForce:"off",
			fullScreenOffsetContainer: "",
			fullScreenOffset: "60px",
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
	try{initSocialSharing("314")} catch(e){}
});	/*ready*/