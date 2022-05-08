function HoverDropdown(id) {
    var x = document.getElementById(id);
    if (x.className.indexOf("w3-show") == -1) {
        x.className += " w3-show";
    } else {
        x.className = x.className.replace(" w3-show", "");
    }
}

function filterFunction(id, mid) {
    var input, filter, ul, li, a, i;
    input = document.getElementById("myInput");
    filter = input.value.toUpperCase();
    div = document.getElementById(id);
    a = div.getElementsByTagName(mid);
    for (i = 0; i < a.length; i++) {
        if (a[i].innerHTML.toUpperCase().indexOf(filter) > -1) {
            a[i].style.display = "";
        } else {
            a[i].style.display = "none";
        }
    }
}

function FilterDivItem(inputid, listid) {
    // Declare variables
    var input, filter, div, li, a, i;
    input = document.getElementById(inputid);
    filter = input.value.toUpperCase();
    div = document.getElementById(listid);
    li = div.getElementsByTagName('div');

    // Loop through all list items, and hide those who don't match the search query
    for (i = 0; i < li.length; i++) {
        a = li[i].getElementsByTagName("p")[0];
        if (a.innerHTML.toUpperCase().indexOf(filter) > -1) {
            li[i].style.display = "";
        } else {
            li[i].style.display = "none";
        }
    }
}

function readIMG(input, demo, img) {
    if (input.files && input.files[0]) {
        var mimeType = input.files[0]['type'];
        if (mimeType.split('/')[0] == 'image') {
            var reader = new FileReader();

            reader.onload = function (e) {
                $(demo).attr('src', e.target.result);
            }

            reader.readAsDataURL(input.files[0]);
            document.getElementById(img).innerHTML = '';
        } else {
            document.getElementById(img).innerHTML = "Please Choose Image to upload.";
            $(input).val(null);
        }

    }
}