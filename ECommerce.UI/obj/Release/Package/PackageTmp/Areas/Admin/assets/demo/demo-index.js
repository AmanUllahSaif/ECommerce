jQuery(document).ready(function() {

    $(".demodrop").pulsate({
        color: "#2bbce0",
        repeat: 10
    });

    $("#threads,#comments,#users").niceScroll({horizrailenabled:false,railoffset: {left:0}});

    //Date Range Picker
    $('#daterangepicker2').daterangepicker(
        {
          ranges: {
             'Today': [moment(), moment()],
             'Yesterday': [moment().subtract('days', 1), moment().subtract('days', 1)],
             'Last 7 Days': [moment().subtract('days', 6), moment()],
             'Last 30 Days': [moment().subtract('days', 29), moment()],
             'This Month': [moment().startOf('month'), moment().endOf('month')],
             'Last Month': [moment().subtract('month', 1).startOf('month'), moment().subtract('month', 1).endOf('month')]
          },
          opens: 'left',
          startDate: moment().subtract('days', 29),
          endDate: moment()
        },
        function(start, end) {
            $('#daterangepicker2 span').html(start.format('MMMM D, YYYY') + ' - ' + end.format('MMMM D, YYYY'));
        }
    );


    //Sparklines

    $("#indexinfocomments").sparkline([12 + randValue(),8 + randValue(),10 + randValue(), 21 + randValue(), 16 + randValue(), 9 + randValue(), 15 + randValue(), 8 + randValue() ,10 + randValue(),19 + randValue()], {
    type: 'bar',
    barColor: '#f1948a',
    height: '45',
    barWidth: 7});

    $("#indexinfolikes").sparkline([120 + randValue(),87 + randValue(),108 + randValue(), 121 + randValue(), 85 + randValue(), 95 + randValue(), 185 + randValue(), 125 + randValue() ,154 + randValue(),109 + randValue()], {
    type: 'bar',
    barColor: '#f5c783',
    height: '45',
    barWidth: 7});
    //Flot

    function randValue() {
        return (Math.floor(Math.random() * (2)));
    }

    var viewcount = [
        [1, 787 + randValue()],
        [2, 740 + randValue()],
        [3, 560 + randValue()],
        [4, 860 + randValue()],
        [5, 750 + randValue()],
        [6, 910 + randValue()],
        [7, 730 + randValue()]
    ];

    var uniqueviews = [
        [1, 179 + randValue()],
        [2, 320 + randValue()],
        [3, 120 + randValue()],
        [4, 400 + randValue()],
        [5, 573 + randValue()],
        [6, 255 + randValue()],
        [7, 366 + randValue()]
    ];

    
    var usercount = [
        [1, 70 + randValue()],
        [2, 260 + randValue()],
        [3, 30 + randValue()],
        [4, 147 + randValue()],
        [5, 333 + randValue()],
        [6, 155 + randValue()],
        [7, 166 + randValue()]
    ];


    var d1 = [
        [1, 29 + randValue()],
        [2, 62 + randValue()],
        [3, 52 + randValue()],
        [4, 41 + randValue()]
    ];
    var d2 = [
        [1, 36 + randValue()],
        [2, 79 + randValue()],
        [3, 66 + randValue()],
        [4, 24 + randValue()]
    ];

    for (var i = 1; i < 5; i++) {
        d1.push([i, parseInt(Math.random() * 1)]);
        d2.push([i, parseInt(Math.random() * 1)]);
    }
 
    var ds = new Array();

    ds.push({
    data:d1,
    label: "Budget",
    bars: {
        show: true,
        barWidth: 0.2,
        order: 1
    }
    });
    ds.push({
        data:d2,
        label: "Actual",
        bars: {
            show: true,
            barWidth: 0.2,
            order: 2,
        }
    });

    var previousPoint = null;
        $("#site-statistics").bind("plothover", function (event, pos, item) {
        $("#x").text(pos.x.toFixed(2));
        $("#y").text(pos.y.toFixed(2));
        if (item) {
            if (previousPoint != item.dataIndex) {
                previousPoint = item.dataIndex;

                $("#tooltip").remove();
                var x = item.datapoint[0].toFixed(2),
                    y = item.datapoint[1].toFixed(2);

                showTooltip(item.pageX, item.pageY-7, item.series.label + ": " + Math.round(y));

            }
        } else {
            $("#tooltip").remove();
            previousPoint = null;
        }
    });

    var previousPointBar = null;
        $("#budget-variance").bind("plothover", function (event, pos, item) {
        $("#x").text(pos.x.toFixed(2));
        $("#y").text(pos.y.toFixed(2));
        if (item) {
            if (previousPointBar != item.dataIndex) {
                previousPointBar = item.dataIndex;

                $("#tooltip").remove();
                var x = item.datapoint[0].toFixed(2),
                    y = item.datapoint[1].toFixed(2);

                showTooltip(item.pageX+20, item.pageY, item.series.label + ": $" + Math.round(y)+"K");

            }
        } else {
            $("#tooltip").remove();
            previousPointBar = null;
        }
    });



    function showTooltip(x, y, contents) {
        $('<div id="tooltip" class="tooltip top in"><div class="tooltip-inner">' + contents + '<\/div><\/div>').css({
            display: 'none',
            top: y - 40,
            left: x - 55,
        }).appendTo("body").fadeIn(200);
    }

});


// Calendar
// If screensize > 1200, render with m/w/d view, if not by default render with just title

renderCalendar({left: 'title',right: 'prev,next'});

enquire.register("screen and (min-width: 1200px)", {
    match : function() {
        $('#calendar-drag').removeData('fullCalendar').empty();
        renderCalendar({left: 'prev,next',center: 'title',right: 'month,basicWeek,basicDay'});
    },
    unmatch : function() {
        $('#calendar-drag').removeData('fullCalendar').empty();
        renderCalendar({left: 'title',right: 'prev,next'});
    }
});


function renderCalendar(headertype) {

    // Demo for FullCalendar with Drag/Drop internal

    var date = new Date();
    var d = date.getDate();
    var m = date.getMonth();
    var y = date.getFullYear();

    var calendar = $('#calendar-drag').fullCalendar({
        header: headertype,
        selectable: true,
        selectHelper: true,
        select: function(start, end, allDay) {
            var title = prompt('Event Title:');
            if (title) {
                calendar.fullCalendar('renderEvent',
                    {
                        title: title,
                        start: start,
                        end: end,
                        allDay: allDay
                    },
                    true // make the event "stick"
                );
            }
            calendar.fullCalendar('unselect');
        },
        editable: true,
        events: [
            {
                title: 'All Day Event',
                start: new Date(y, m, 8),
                backgroundColor: '#efa131'
            },
            {
                title: 'Long Event',
                start: new Date(y, m, d-5),
                end: new Date(y, m, d-2),
                backgroundColor: '#7a869c'
            },
            {
                id: 999,
                title: 'Repeating Event',
                start: new Date(y, m, d-3, 16, 0),
                allDay: false,
                backgroundColor: '#e74c3c'
            },
            {
                id: 999,
                title: 'Repeating Event',
                start: new Date(y, m, d+4, 16, 0),
                allDay: false,
                backgroundColor: '#e74c3c'
            },
            {
                title: 'Meeting',
                start: new Date(y, m, d, 10, 30),
                allDay: false,
                backgroundColor: '#76c4ed'
            },
            {
                title: 'Lunch',
                start: new Date(y, m, d, 12, 0),
                end: new Date(y, m, d, 14, 0),
                allDay: false,
                backgroundColor: '#34495e'
            },
            {
                title: 'Birthday Party',
                start: new Date(y, m, d+1, 19, 0),
                end: new Date(y, m, d+1, 22, 30),
                allDay: false,
                backgroundColor: '#2bbce0'
            },
            {
                title: 'Click for Google',
                start: new Date(y, m, 28),
                end: new Date(y, m, 29),
                url: 'http://google.com/',
                backgroundColor: '#f1c40f'
            }
        ],
        buttonText: {
            prev: '<i class="fa fa-angle-left"></i>',
            next: '<i class="fa fa-angle-right"></i>',
            prevYear: '<i class="fa fa-angle-double-left"></i>',  // <<
            nextYear: '<i class="fa fa-angle-double-right"></i>',  // >>
            today:    'Today',
            month:    'Month',
            week:     'Week',
            day:      'Day'
        }
    });

    // Listen for click on toggle checkbox
    $('#select-all').click(function(event) {
        if(this.checked) {
            $('.selects :checkbox').each(function() {
                this.checked = true;
            });
        } else {
            $('.selects :checkbox').each(function() {
                this.checked = false;
            });
        }
    });

    //$( ".panel-tasks" ).sortable({placeholder: 'item-placeholder'});
    $('.panel-tasks input[type="checkbox"]').click(function(event) {
        if(this.checked) {
            $(this).next(".task-description").addClass("done");
        } else {
            $(this).next(".task-description").removeClass("done");
        }
    });

}