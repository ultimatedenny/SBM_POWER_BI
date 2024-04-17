$(function() {
	$('.captcha-holder').iconCaptcha({
		captchaTheme: ["light"], 
		captchaFontFamily: '',
		captchaClickDelay: 500,
		captchaHoverDetection: true,
		enableLoadingAnimation: true,
		loadingAnimationDelay: 1500,
		showCredits: 'show',
		requestIconsDelay: 1500,
		captchaAjaxFile: '~/Captcha/GetCaptcha/',
		captchaMessages: {
			header: "Select the image that does not belong in the row",
			correct: {
				top: "Great!",
				bottom: "You do not appear to be a robot."
			},
			incorrect: {
				top: "Oops!",
				bottom: "You've selected the wrong image."
			}
		}
	});
});