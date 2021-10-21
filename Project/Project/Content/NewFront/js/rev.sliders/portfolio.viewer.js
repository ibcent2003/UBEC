var tpj=jQuery;
			var revapi10;
			tpj(document).ready(function() {
				if(tpj("#rev_slider_10_1").revolution == undefined){
					revslider_showDoubleJqueryError("#rev_slider_10_1");
				}else{
					revapi10 = tpj("#rev_slider_10_1").show().revolution({
						sliderType:"standard",
						jsFileLocation:"revolution/js/",
						sliderLayout:"fullscreen",
						dottedOverlay:"none",
						delay:5000,
						particles: {startSlide: "first", endSlide: "last", zIndex: "1",
							particles: {
								number: {value: 200}, color: {value: "#ffffff"},
								shape: {
									type: "circle", stroke: {width: 0, color: "#ffffff", opacity: 1},
									image: {src: ""}
								},
								opacity: {value: 0.1, random: false, min: 0.25, anim: {enable: false, speed: 1, opacity_min: 0, sync: false}},
								size: {value: 1, random: true, min: 0.5, anim: {enable: false, speed: 40, size_min: 1, sync: false}},
								line_linked: {enable: true, distance: 30, color: "#ffffff", opacity: 0.75, width: 1},
								move: {enable: true, speed: 1, direction: "right", random: true, min_speed: 1, straight: false, out_mode: "out"}},
							interactivity: {
								events: {onhover: {enable: false, mode: "repulse"}, onclick: {enable: false, mode: "bubble"}},
								modes: {grab: {distance: 400, line_linked: {opacity: 0.5}}, bubble: {distance: 400, size: 100, opacity: 1}, repulse: {distance: 75}}
							}
						},
						navigation: {
							onHoverStop:"off",
						},
						responsiveLevels:[1240,1024,778,480],
						visibilityLevels:[1240,1024,778,480],
						gridwidth:[1240,1024,778,480],
						gridheight:[868,768,960,720],
						lazyType:"none",
						parallax: {
							type:"scroll",
							origo:"slidercenter",
							speed:400,
							levels:[5,10,15,20,25,30,35,40,45,46,47,48,49,50,51,5],
						},
						shadow:0,
						spinner:"off",
						stopLoop:"on",
						stopAfterLoops:0,
						stopAtSlide:1,
						shuffle:"off",
						autoHeight:"off",
						fullScreenAutoWidth:"off",
						fullScreenAlignForce:"off",
						fullScreenOffsetContainer: "",
						fullScreenOffset: "",
						hideThumbsOnMobile:"off",
						hideSliderAtLimit:0,
						hideCaptionAtLimit:0,
						hideAllCaptionAtLilmit:0,
						debugMode:false,
						fallbacks: {
							simplifyAll:"off",
							nextSlideOnWindowFocus:"off",
							disableFocusListener:false,
						}
					});
				var is_safari = (navigator.userAgent.indexOf('Safari') != -1 && navigator.userAgent.indexOf('Chrome') == -1);

				revapi10.bind("revolution.slide.onloaded",function (e) {
					var npe = document.getElementsByClassName("nopointerevent");
					for (var i=0;i<npe.length;i++) {
						npe[i].parentNode.parentNode.parentNode.style.pointerEvents = "none";
						npe[i].parentNode.parentNode.parentNode.style.zIndex = 100;
					}
												   
					jQuery('.ddd_mousebox').on('mousemove',function(e) {       
						var sto = revapi10.offset(),
							dim = this.getBoundingClientRect(),
							tpos = jQuery(this.parentNode.parentNode.parentNode).position(),
							pos = {top:e.pageY-sto.top-tpos.top,left:e.pageX-tpos.left},
							perc = {wp:(pos.left/dim.width)-0.5, hp:(pos.top/dim.height)-0.5};
						getOverlaps(this);
						punchgs.TweenLite.to(this.overlapps,0.4,{force3D:"true",overwrite:"auto",transformOrigin:"50% 50% 100%", z:"300px",rotationY:0-((perc.wp)*5),rotationX:((perc.hp)*5),zIndex:30});
						punchgs.TweenLite.set(this.parentNode.parentNode.parentNode,{zIndex:10});
						if (is_safari)
							punchgs.TweenLite.to(this,0.4,{force3D:"true",overwrite:"auto",z:"10px",transformOrigin:"50% 50%", rotationY:0-((perc.wp)*10),rotationX:((perc.hp)*10)});
						else
						   punchgs.TweenLite.to(this,0.4,{force3D:"true",overwrite:"auto",z:"10px",transformOrigin:"50% 50%", rotationY:0-((perc.wp)*10),rotationX:((perc.hp)*10),boxShadow:"0 50px 100px rgba(15,20,40,0.35),0 20px 45px rgba(15,20,40,0.35)"});
					});

					jQuery('.ddd_mousebox').on('mouseleave',function(e) {
						punchgs.TweenLite.set(this.parentNode.parentNode.parentNode,{zIndex:5});
					   punchgs.TweenLite.to(this,0.5,{force3D:"true",overwrite:"auto",z:"0px",transformOrigin:"50% 50%", rotationY:0,rotationX:0,boxShadow:"0,0,0,0 rgba(0,0,0,0)"});
					   punchgs.TweenLite.to(this.overlapps,0.5,{force3D:"true",z:"0px",overwrite:"auto",transformOrigin:"50% 50% 100%", rotationY:0,rotationX:0});
					});
					 
				});
												   
				function getOverlaps(el) {
				  if (el.overlapps == undefined) {
					el.overlapps = [];

				  } 
				}				
				
			}

            RsParticlesAddOn(revapi10);
			});	/*ready*/