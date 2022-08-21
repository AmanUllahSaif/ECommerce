jQuery(document).ready(function ($) {
    //open/close mega-navigation
    $('.cd-dropdown-trigger').on('click', function (event) {
        var id = $(this).attr('href');
        event.preventDefault();
        var showNav = $(id).hasClass('dropdown-is-active');
        if ($('.cd-dropdown').hasClass('dropdown-is-active')) {
            hideNav();
        }
        if (!showNav) {
            toggleNav(id);
        }
    });

    //close meganavigation
    $('.cd-dropdown .cd-close').on('click', function (event) {
        var id = $(this).attr('href');
        event.preventDefault();
        toggleNav(id);
    });

    //on mobile - open submenu
    $('.has-children').children('a').on('click', function (event) {
        //prevent default clicking on direct children of .has-children 
        event.preventDefault();
        var selected = $(this);
        selected.next('ul').removeClass('is-hidden').end().parent('.has-children').parent('ul').addClass('move-out');
    });

    //on desktop - differentiate between a user trying to hover over a dropdown item vs trying to navigate into a submenu's contents
    var submenuDirection = (!$('.cd-dropdown-wrapper').hasClass('open-to-left')) ? 'right' : 'left';
    $('.cd-dropdown-content').menuAim({
        activate: function (row) {
            $(row).children().addClass('is-active').removeClass('fade-out');
            if ($('.cd-dropdown-content .fade-in').length == 0) $(row).children('ul').addClass('fade-in');
        },
        deactivate: function (row) {
            $(row).children().removeClass('is-active');
            if ($('li.has-children:hover').length == 0 || $('li.has-children:hover').is($(row))) {
                $('.cd-dropdown-content').find('.fade-in').removeClass('fade-in');
                $(row).children('ul').addClass('fade-out')
            }
        },
        exitMenu: function () {
            $('.cd-dropdown-content').find('.is-active').removeClass('is-active');
            return true;
        },
        submenuDirection: submenuDirection,
    });

    //submenu items - go back link
    $('.go-back').on('click', function () {
        var selected = $(this),
			visibleNav = $(this).parent('ul').parent('.has-children').parent('ul');
        selected.parent('ul').addClass('is-hidden').parent('.has-children').parent('ul').removeClass('move-out');
    });

    function toggleNav(e) {
        var navIsVisible = (!$(e).hasClass('dropdown-is-active')) ? true : false;
        $(e).toggleClass('dropdown-is-active', navIsVisible);
        $(e).parent('.cd-dropdown-trigger').toggleClass('dropdown-is-active', navIsVisible);
        if (!navIsVisible) {
            $('.cd-dropdown').one('webkitTransitionEnd otransitionend oTransitionEnd msTransitionEnd transitionend', function () {
                $('.has-children ul').addClass('is-hidden');
                $('.move-out').removeClass('move-out');
                $('.is-active').removeClass('is-active');
            });
        }
    }

    function hideNav() {
        var navIsVisible = false;
        $('.cd-dropdown').toggleClass('dropdown-is-active', navIsVisible);
        $('.cd-dropdown-trigger').toggleClass('dropdown-is-active', navIsVisible);
        if (!navIsVisible) {
            $('.cd-dropdown').one('webkitTransitionEnd otransitionend oTransitionEnd msTransitionEnd transitionend', function () {
                $('.has-children ul').addClass('is-hidden');
                $('.move-out').removeClass('move-out');
                $('.is-active').removeClass('is-active');
            });
        }
    }


});

//mega nav box

jQuery(document).on('click', '.mega-dropdown', function (e) {
    e.stopPropagation()
})



$('.dropdown-toggle').click(function () {
    var postion = $(this).position();
    var css = 'left :' + postion.left+' !important'
})