function formatCurrency(value) {
    return accounting.formatNumber(value) + " VNĐ";
}

var handler = function (element, valueAccessor, allBindings) {
    var $el = $(element);
    var method;

    // Gives us the real value if it is a computed observable or not
    var valueUnwrapped = ko.unwrap(valueAccessor());

    if ($el.is(':input')) {
        method = 'val';
    } else {
        method = 'text';
    }
    return $el[method](formatCurrency(valueUnwrapped));
};

ko.bindingHandlers.money = {
    update: handler
};

function checkMobile() {
    if (/Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent)) {
        return true;
    }
    
    return false;
}

function initSliderOption(id) {
    var slideshowTransitions = [{ $Duration: 1200, $Opacity: 2 }];
    var options = {
        $AutoPlay: true,
        $AutoPlaySteps: 1,
        $AutoPlayInterval: 5000,
        $PauseOnHover: 1,
        $ArrowKeyNavigation: true,
        $SlideDuration: 500,
        $MinDragOffsetToSlide: 20,
        $SlideSpacing: 0,
        $DisplayPieces: 1,
        $ParkingPosition: 0,
        $UISearchMode: 1,
        $PlayOrientation: 1,
        $DragOrientation: 3,
        $SlideshowOptions: {
            $Class: $JssorSlideshowRunner$,
            $Transitions: slideshowTransitions,
            $TransitionsOrder: 1,
            $ShowLink: true
        },
        $BulletNavigatorOptions: {
            $Class: $JssorBulletNavigator$,
            $ChanceToShow: 2,
            $AutoCenter: 1,
            $Steps: 1,
            $Lanes: 1,
            $SpacingX: 10,
            $SpacingY: 10,
            $Orientation: 1
        },
        $ArrowNavigatorOptions: {
            $Class: $JssorArrowNavigator$,
            $ChanceToShow: 1,
            $Steps: 1
        }
    };
    var jssor_slider = new $JssorSlider$(id, options);

    function scaleSlider() {
        var refSize = jssor_slider.$Elmt.parentNode.clientWidth;
        if (refSize) {
            refSize = Math.min(refSize, 1920);
            jssor_slider.$ScaleWidth(refSize);
        }
        else {
            window.setTimeout(scaleSlider, 30);
        }
    }
    scaleSlider();
    $Jssor$.$AddEvent(window, "load", scaleSlider);
    $Jssor$.$AddEvent(window, "resize", scaleSlider);
    $Jssor$.$AddEvent(window, "orientationchange", scaleSlider);
}

function getGridSize() {
    return (window.innerWidth < 400) ? 1 :
           (window.innerWidth < 600) ? 2 :
           (window.innerWidth < 900) ? 3 : 5;
}

function convertObj(ix) {
    var dx = {};
    dx = ix;
    return dx;
}