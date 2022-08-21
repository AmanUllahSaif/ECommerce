var canvas;
var tshirts = new Array(); //prototype: [{style:'x',color:'white',front:'a',back:'b',price:{tshirt:'12.95',frontPrint:'4.99',backPrint:'4.99',total:'22.47'}}]
var a;
var b;
var line1;
var line2;
var line3;
var line4;
var canvasSide = 1;
var shirt;
$(document).ready(function () {
    //setup front side canvas 
    canvas = new fabric.Canvas('tcanvas', {
        hoverCursor: 'pointer',
        selection: true,
        selectionBorderColor: 'blue'
    });


    canvas.on({
        'object:moving': function (e) {
            e.target.opacity = 0.5;
        },
        'object:modified': function (e) {
            e.target.opacity = 1;
        },
        'object:selected': onObjectSelected,
        'selection:cleared': onSelectedCleared
    });
    // piggyback on `canvas.findTarget`, to fire "object:over" and "object:out" events
    canvas.findTarget = (function (originalFn) {
        return function () {
            var target = originalFn.apply(this, arguments);
            if (target) {
                if (this._hoveredTarget !== target) {
                    canvas.fire('object:over', { target: target });
                    if (this._hoveredTarget) {
                        canvas.fire('object:out', { target: this._hoveredTarget });
                    }
                    this._hoveredTarget = target;
                }
            }
            else if (this._hoveredTarget) {
                canvas.fire('object:out', { target: this._hoveredTarget });
                this._hoveredTarget = null;
            }
            return target;
        };
    })(canvas.findTarget);

    canvas.on('object:over', function (e) {
        //e.target.setFill('red');
        //canvas.renderAll();
    });

    canvas.on('object:out', function (e) {
        //e.target.setFill('green');
        //canvas.renderAll();
    });

    document.getElementById('add-text').onclick = function () {
        var text = $("#text-string").val();
        var textSample = new fabric.Text(text, {
            left: fabric.util.getRandomInt(0, 200),
            top: fabric.util.getRandomInt(0, 450),
            fontFamily: 'helvetica',
            angle: 0,
            fill: '#000000',
            scaleX: 0.5,
            scaleY: 0.5,
            fontWeight: '',
            hasRotatingPoint: true
        });
        canvas.add(textSample);
        canvas.item(canvas.item.length - 1).hasRotatingPoint = true;
        setPrice();
        $("#texteditor").css('display', 'block');
        $("#imageeditor").css('display', 'block');
    };
    $("#text-string").keyup(function () {
        var activeObject = canvas.getActiveObject();
        if (activeObject && activeObject.type === 'text') {
            activeObject.text = this.value;
            canvas.renderAll();
        }
    });
    $(document).on('click', '.img-polaroid', function (e) {
        var el = e.target;
        /*temp code*/
        var offset = 50;
        var left = fabric.util.getRandomInt(0 + offset, 200 - offset);
        var top = fabric.util.getRandomInt(0 + offset, 450 - offset);
        var angle = fabric.util.getRandomInt(-20, 40);
        var width = fabric.util.getRandomInt(30, 50);
        var opacity = (function (min, max) { return Math.random() * (max - min) + min; })(0.5, 1);

        fabric.Image.fromURL(el.src, function (image) {
            image.set({
                left: left,
                top: top,
                angle: 0,
                padding: 10,
                cornersize: 10,
                hasRotatingPoint: true
            });
            //image.scale(getRandomNum(0.1, 0.25)).setCoords();
            canvas.add(image);
            setPrice();
        });
    });
    document.getElementById('remove-selected').onclick = function () {
        var activeObject = canvas.getActiveObject(),
            activeGroup = canvas.getActiveGroup();
        if (activeObject) {
            canvas.remove(activeObject);
            $("#text-string").val("");
        }
        else if (activeGroup) {
            var objectsInGroup = activeGroup.getObjects();
            canvas.discardActiveGroup();
            objectsInGroup.forEach(function (object) {
                canvas.remove(object);
            });
        }
        setPrice();
    };
    document.getElementById('bring-to-front').onclick = function () {
        var activeObject = canvas.getActiveObject(),
            activeGroup = canvas.getActiveGroup();
        if (activeObject) {
            activeObject.bringToFront();
        }
        else if (activeGroup) {
            var objectsInGroup = activeGroup.getObjects();
            canvas.discardActiveGroup();
            objectsInGroup.forEach(function (object) {
                object.bringToFront();
            });
        }
    };
    document.getElementById('send-to-back').onclick = function () {
        var activeObject = canvas.getActiveObject(),
            activeGroup = canvas.getActiveGroup();
        if (activeObject) {
            activeObject.sendToBack();
        }
        else if (activeGroup) {
            var objectsInGroup = activeGroup.getObjects();
            canvas.discardActiveGroup();
            objectsInGroup.forEach(function (object) {
                object.sendToBack();
            });
        }
    };
    $("#text-bold").click(function () {
        var activeObject = canvas.getActiveObject();
        if (activeObject && activeObject.type === 'text') {
            activeObject.fontWeight = (activeObject.fontWeight == 'bold' ? '' : 'bold');
            canvas.renderAll();
        }
    });
    $("#text-italic").click(function () {
        var activeObject = canvas.getActiveObject();
        if (activeObject && activeObject.type === 'text') {
            activeObject.fontStyle = (activeObject.fontStyle == 'italic' ? '' : 'italic');
            canvas.renderAll();
        }
    });
    $("#text-strike").click(function () {
        var activeObject = canvas.getActiveObject();
        if (activeObject && activeObject.type === 'text') {
            activeObject.textDecoration = (activeObject.textDecoration == 'line-through' ? '' : 'line-through');
            canvas.renderAll();
        }
    });
    $("#text-underline").click(function () {
        var activeObject = canvas.getActiveObject();
        if (activeObject && activeObject.type === 'text') {
            activeObject.textDecoration = (activeObject.textDecoration == 'underline' ? '' : 'underline');
            canvas.renderAll();
        }
    });
    $("#text-left").click(function () {
        var activeObject = canvas.getActiveObject();
        if (activeObject && activeObject.type === 'text') {
            activeObject.textAlign = 'left';
            canvas.renderAll();
        }
    });
    $("#text-center").click(function () {
        var activeObject = canvas.getActiveObject();
        if (activeObject && activeObject.type === 'text') {
            activeObject.textAlign = 'center';
            canvas.renderAll();
        }
    });
    $("#text-right").click(function () {
        var activeObject = canvas.getActiveObject();
        if (activeObject && activeObject.type === 'text') {
            activeObject.textAlign = 'right';
            canvas.renderAll();
        }
    });
    $("#font-family").change(function () {
        var activeObject = canvas.getActiveObject();
        if (activeObject && activeObject.type === 'text') {
            activeObject.fontFamily = this.value;
            canvas.renderAll();
        }
    });
    $('#text-bgcolor').minicolors({
        change: function (hex, rgb) {
            var activeObject = canvas.getActiveObject();
            if (activeObject && activeObject.type === 'text') {
                activeObject.backgroundColor = this.value;
                canvas.renderAll();
            }
        },
        open: function (hex, rgb) {
            //
        },
        close: function (hex, rgb) {
            //
        }
    });
    //$('#text-fontcolor').on('change', function () {
    //    debugger;
    //    // This shows how to obtain the hex color and opacity (when in use)
    //    var hex = $(this).val();
    //    var activeObject = canvas.getActiveObject();
    //    if (activeObject && activeObject.type === 'text') {
    //        activeObject.fill = $(this).val();
    //        canvas.renderAll();
    //    }

    //});
    $('#text-fontcolor').minicolors({
        change: function (hex, rgb) {
            var activeObject = canvas.getActiveObject();
            if (activeObject && activeObject.type === 'text') {
                activeObject.fill = this.value;
                canvas.renderAll();
            }
        },
        open: function (hex, rgb) {
            //
        },
        close: function (hex, rgb) {
            //
        }
    });

    $('#text-strokecolor').minicolors({
        change: function (hex, rgb) {
            debugger;
            var activeObject = canvas.getActiveObject();
            if (activeObject && activeObject.type === 'text') {
                activeObject.strokeStyle = this.value;
                canvas.renderAll();
            }
        },
        open: function (hex, rgb) {
            //
        },
        close: function (hex, rgb) {
            //
        }
    });

    //canvas.add(new fabric.fabric.Object({hasBorders:true,hasControls:false,hasRotatingPoint:false,selectable:false,type:'rect'}));
    $("#drawingArea").hover(
        function () {
            canvas.add(line1);
            canvas.add(line2);
            canvas.add(line3);
            canvas.add(line4);
            canvas.renderAll();
        },
        function () {
            canvas.remove(line1);
            canvas.remove(line2);
            canvas.remove(line3);
            canvas.remove(line4);
            canvas.renderAll();
        }
    );

    $('.color-preview').click(function () {
        var color = $(this).css("background-color");
        document.getElementById("shirtDiv").style.backgroundColor = color;
        $('#hdnColor').val(color);
    });

    $('#flip').click(
        function () {
            if (shirt) {
                if ($(this).attr("data-original-title") == "Show Back View") {
                    canvasSide = 2;
                    $(this).attr('data-original-title', 'Show Front View');
                    $("#tshirtFacing").attr("src", shirt.BackImageUrl);
                    a = JSON.stringify(canvas);
                    canvas.clear();
                    try {
                        var json = JSON.parse(b);
                        canvas.loadFromJSON(b);
                    }
                    catch (e)
                    { }

                } else {
                    canvasSide = 1;
                    $(this).attr('data-original-title', 'Show Back View');
                    $("#tshirtFacing").attr("src", shirt.FrontImageUrl);
                    b = JSON.stringify(canvas);
                    canvas.clear();
                    try {
                        var json = JSON.parse(a);
                        canvas.loadFromJSON(a);
                    }
                    catch (e)
                    { }
                }
                canvas.renderAll();
                setTimeout(function () {
                    canvas.calcOffset();
                    setPrice();
                }, 200);
            }
        });
    $(".clearfix button,a").tooltip();
    line1 = new fabric.Line([0, 0, 200, 0], { "stroke": "#000000", "strokeWidth": 1, hasBorders: false, hasControls: false, hasRotatingPoint: false, selectable: false });
    line2 = new fabric.Line([199, 0, 200, 449], { "stroke": "#000000", "strokeWidth": 1, hasBorders: false, hasControls: false, hasRotatingPoint: false, selectable: false });
    line3 = new fabric.Line([0, 0, 0, 450], { "stroke": "#000000", "strokeWidth": 1, hasBorders: false, hasControls: false, hasRotatingPoint: false, selectable: false });
    line4 = new fabric.Line([0, 450, 200, 449], { "stroke": "#000000", "strokeWidth": 1, hasBorders: false, hasControls: false, hasRotatingPoint: false, selectable: false });

    $('#addToTheBag').click(function () {

    });
});//doc ready


function getRandomNum(min, max) {
    return Math.random() * (max - min) + min;
}

function onObjectSelected(e) {
    var selectedObject = e.target;
    $("#text-string").val("");
    selectedObject.hasRotatingPoint = true
    if (selectedObject && selectedObject.type === 'text') {
        //display text editor	    	
        $("#texteditor").css('display', 'block');
        $("#text-string").val(selectedObject.getText());
        $('#text-fontcolor').minicolors('value', selectedObject.fill);
        $('#text-strokecolor').minicolors('value', selectedObject.strokeStyle);
        $("#imageeditor").css('display', 'block');
    }
    else if (selectedObject && selectedObject.type === 'image') {
        //display image editor
        $("#texteditor").css('display', 'none');
        $("#imageeditor").css('display', 'block');
    }
}
function onSelectedCleared(e) {
    $("#texteditor").css('display', 'none');
    $("#text-string").val("");
    // $("#imageeditor").css('display', 'none');
}
function setFont(font) {
    var activeObject = canvas.getActiveObject();
    if (activeObject && activeObject.type === 'text') {
        activeObject.fontFamily = font;
        canvas.renderAll();
    }
}
function removeWhite() {
    var activeObject = canvas.getActiveObject();
    if (activeObject && activeObject.type === 'image') {
        activeObject.filters[2] = new fabric.Image.filters.RemoveWhite({ hreshold: 100, distance: 10 });//0-255, 0-255
        activeObject.applyFilters(canvas.renderAll.bind(canvas));
    }
}

$('#Type').change(function () {
    var type = $(this).val();
    if (type) {
        $('#shirtType').html($('#Type option:selected').text());
        $.ajax({
            url: '/shopping/GetShirtDetail/' + type,
            type: 'GET',
            success: function (data) {
                
                shirt = data;
                $("#tshirtFacing").attr("src", shirt.FrontImageUrl);
                $('#cost').html(shirt.ShirtPrice);
                $('#ShirtPrice').val(shirt.ShirtPrice);
                $('#infoShirt').hide();
                $('#tshirtFacing').show();
            }
        })
    } else {
        $('#infoShirt').show();
        $('#tshirtFacing').hide();
    }
    
})

function SetValuesOfFrontBack() {
    if (canvasSide == 1) {
        var frontObj = canvas.getObjects();
        if (frontObj.length > 0) {
            var img = canvas.toDataURL('png')
            $('#FrontImageUrl').val(img);
        } else {
            $('#FrontImageUrl').val('');
        }
        var bparse = JSON.parse(b);
        if (bparse != undefined && bparse.objects.length > 0) {
            a = JSON.stringify(canvas);
            var json = JSON.parse(b);
            canvas.loadFromJSON(b);
            canvas.renderAll();
            var backImg = canvas.toDataURL('png');
            $('#BackImageUrl').val(backImg);

            json = JSON.parse(a);
            canvas.loadFromJSON(a);
        } else {
            $('#BackImageUrl').val('');
        }
    } else {
        var backObject = canvas.getObjects();
        if (backObject.length > 0) {
            var backImg = canvas.toDataURL('png');
            $('#BackImageUrl').val(backImg);
        } else {
            $('#BackImageUrl').val('');
        }

        var aparse = JSON.parse(a);
        if (aparse != undefined && aparse.objects.length > 0) {
            a = JSON.stringify(canvas);
            var json = JSON.parse(a);
            canvas.loadFromJSON(a);
            canvas.renderAll();
            var backImg = canvas.toDataURL('png');
            $('#FrontImageUrl').val(backImg);

            json = JSON.parse(b);
            canvas.loadFromJSON(b);
        } else {

            $('#FrontImageUrl').val('');
        }

    }
    debugger;
    var type = $('#Type').val();
    if (type) {
        return true;
    } else {
        return false;
    }
}

function encodeImageFileAsURL(element) {
    var file = element.files[0];
    var reader = new FileReader();
    reader.onloadend = function () {
        var img = '<img style="cursor:pointer;height:100px;width:100px" class="img-polaroid" src="' + reader.result + '"/>'
        $('#avatarlist').append(img);
    }
    reader.readAsDataURL(file);
}

function setPrice() {
    if (shirt) {

        var total = shirt.ShirtPrice;
        if (canvasSide == 1) {
            var objs = canvas.getObjects();
            if (objs.length > 0) {
                $('#fcost').html(shirt.FrontPrintPrice);
                $('#FrontPrice').val(shirt.FrontPrintPrice);
                total += shirt.FrontPrintPrice;
            } else {
                $('#fcost').html('0.00');
                $('#FrontPrintPrice').val('');
            }
            if (b) {
                var bparse = JSON.parse(b);
                if (bparse != undefined && bparse.objects.length > 0) {
                    $('#bcost').html(shirt.BackPrintPrice);
                    $('#BackPrice').val(shirt.BackPrintPrice);
                    total += shirt.BackPrintPrice;
                }
            } else {
                $('#bcost').html('0.00');
                $('#BackPrintPrice').val('');
            }
        } else {
            var objs = canvas.getObjects();
            if (objs.length > 0) {
                $('#bcost').html(shirt.BackPrintPrice);
                $('#BackPrintPrice').val(shirt.BackPrintPrice);
                total += shirt.BackPrintPrice;
            } else {
                $('#bcost').html('0.00');
                $('#BackPrintPrice').val('');
            }
            if (a) {
                var aparse = JSON.parse(a);
                if (aparse != undefined && aparse.objects.length > 0) {
                    $('#fcost').html(shirt.FrontPrintPrice);
                    $('#FrontPrintPrice').val(shirt.FrontPrintPrice);
                    total += shirt.FrontPrintPrice;
                }
            } else {
                $('#fcost').html('0.00');
                $('#FrontPrintPrice').val('');
            }
        }
        var qty = parseInt($('#Quantity').val());
        $('#total').html(qty*total);
    }
}