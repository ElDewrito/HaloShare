var Editor = (function () {
    function Editor(editorId) {
        var _this = this;
        this.text = document.getElementById(editorId);
        $(".btn-editor").on("click", function (e) { return _this.onClick(e); });
    }
    Editor.prototype.onClick = function (e) {
        event.preventDefault();
        var target = $(e.target);
        var type = target.attr("data-type");
        switch (type) {
            case "bold":
                this.doTag("b");
                break;
            case "italic":
                this.doTag("i");
                break;
            case "underline":
                this.doTag("u");
                break;
            case "strikethrough":
                this.doTag("s");
                break;
            case "header":
                this.doTag("header");
                break;
            case "horizontal-ruler":
                this.doTag("hr");
                break;
            case "align-left":
                this.doTag("align", "left");
                break;
            case "align-center":
                this.doTag("align", "center");
                break;
            case "align-right":
                this.doTag("align", "right");
                break;
            case "align-justify":
                this.doTag("align", "justify");
                break;
            case "link":
                this.doUrl();
                break;
            case "image":
                this.doImage();
                break;
            case "youtube":
                this.doYoutube();
                break;
            case "list-ordered":
                this.doList(true);
                break;
            case "list-unordered":
                this.doList(false);
                break;
            case "gallery":
                this.doGallery();
                break;
        }
    };
    Editor.prototype.doTag = function (tag, parameter) {
        var startTag;
        if (parameter == undefined)
            startTag = "[" + tag + "]";
        else
            startTag = "[" + tag + "=" + parameter + "]";
        var endTag = "[/" + tag + "]";
        if (document.selection) {
            this.text.focus();
            var selectedRange = document.selection.createRange();
            selectedRange.text = startTag + selectedRange.text + endTag;
        }
        else {
            var len = this.text.value.length;
            var start = this.text.selectionStart;
            var end = this.text.selectionEnd;
            var selectedText = this.text.value.substring(start, end);
            var scrollTop = this.text.scrollTop;
            var scrollLeft = this.text.scrollLeft;
            var rep = startTag + selectedText + endTag;
            this.text.value = this.text.value.substring(0, start) + rep + this.text.value.substring(end, len);
            this.text.scrollTop = scrollTop;
            this.text.scrollLeft = scrollLeft;
        }
    };
    Editor.prototype.doUrl = function () {
        var url = prompt('Enter the URL:', 'http://');
        if (url == null)
            return;
        var scrollTop = this.text.scrollTop;
        var scrollLeft = this.text.scrollLeft;
        if (document.selection) {
            this.text.focus();
            var selectedRange = document.selection.createRange();
            if (selectedRange.text == "") {
                selectedRange.text = '[url]' + url + '[/url]';
            }
            else {
                selectedRange.text = '[url=' + url + ']' + selectedRange.text + '[/url]';
            }
        }
        else {
            var len = this.text.value.length;
            var start = this.text.selectionStart;
            var end = this.text.selectionEnd;
            var sel = this.text.value.substring(start, end);
            if (sel == "") {
                var rep = '[url]' + url + '[/url]';
            }
            else {
                var rep = '[url=' + url + ']' + sel + '[/url]';
            }
            this.text.value = this.text.value.substring(0, start) + rep + this.text.value.substring(end, len);
            this.text.scrollTop = scrollTop;
            this.text.scrollLeft = scrollLeft;
        }
    };
    Editor.prototype.doImage = function () {
        var url = prompt('Enter the Image URL:', 'http://');
        if (url == null)
            return;
        var scrollTop = this.text.scrollTop;
        var scrollLeft = this.text.scrollLeft;
        if (document.selection) {
            this.text.focus();
            var selectedRange = document.selection.createRange();
            selectedRange.text = '[img]' + url + '[/img]';
        }
        else {
            var len = this.text.value.length;
            var start = this.text.selectionStart;
            var end = this.text.selectionEnd;
            var sel = this.text.value.substring(start, end);
            var rep = '[img]' + url + '[/img]';
            this.text.value = this.text.value.substring(0, start) + rep + this.text.value.substring(end, len);
            this.text.scrollTop = scrollTop;
            this.text.scrollLeft = scrollLeft;
        }
    };
    Editor.prototype.doYoutube = function () {
        var url = prompt('Enter the YouTube URL:', 'http://');
        if (url == null)
            return;
        var id = "";
        var scrollTop = this.text.scrollTop;
        var scrollLeft = this.text.scrollLeft;
        var regExp = /^.*(youtu.be\/|v\/|u\/\w\/|embed\/|watch\?v=)([^#\&\?]*).*/;
        var match = url.match(regExp);
        if (match && match[2].length == 11) {
            id = match[2];
        }
        else {
            alert("The URL you entered is incorrect.");
            return;
        }
        if (document.selection) {
            this.text.focus();
            var selectedRange = document.selection.createRange();
            selectedRange.text = '[youtube]' + id + '[/youtube]';
        }
        else {
            var len = this.text.value.length;
            var start = this.text.selectionStart;
            var end = this.text.selectionEnd;
            var sel = this.text.value.substring(start, end);
            var rep = '[youtube]' + id + '[/youtube]';
            this.text.value = this.text.value.substring(0, start) + rep + this.text.value.substring(end, len);
            this.text.scrollTop = scrollTop;
            this.text.scrollLeft = scrollLeft;
        }
    };
    Editor.prototype.doGallery = function () {
        var startTag = "[gallery]";
        var endTag = "[/gallery]";
        if (document.selection) {
            this.text.focus();
            var selectedRange = document.selection.createRange();
            var list = selectedRange.text.split('\n');
            for (i = 0; i < list.length; i++) {
                list[i] = list[i];
            }
            var str = "";
            do {
                if (list[list.length - 1] == "")
                    list.pop();
                var str = prompt("Add Item", "");
                if (str != null)
                    list[list.length] = str;
            } while (str);
            if (list[list.length - 1] == "")
                list.pop();
            selectedRange.text = startTag + '\n' + list.join("\n") + '\n' + endTag;
        }
        else {
            var len = this.text.value.length;
            var start = this.text.selectionStart;
            var end = this.text.selectionEnd;
            var i;
            var scrollTop = this.text.scrollTop;
            var scrollLeft = this.text.scrollLeft;
            var sel = this.text.value.substring(start, end);
            var list = sel.split('\n');
            for (i = 0; i < list.length; i++) {
                list[i] = list[i];
            }
            var str = "";
            do {
                if (list[list.length - 1] == "")
                    list.pop();
                var str = prompt("Enter a gallery item.\n\nLeave the box empty or press 'Cancel' to complete this list:", "");
                if (str != null)
                    list[list.length] = str;
            } while (str);
            if (list[list.length - 1] == "")
                list.pop();
            var rep = startTag + '\n' + list.join("\n") + '\n' + endTag;
            this.text.value = this.text.value.substring(0, start) + rep + this.text.value.substring(end, len);
            this.text.scrollTop = scrollTop;
            this.text.scrollLeft = scrollLeft;
        }
    };
    Editor.prototype.doList = function (isOrdered) {
        var startTag = "[list]";
        if (isOrdered == true)
            startTag = "[list=1]";
        var endTag = "[/list]";
        if (document.selection) {
            this.text.focus();
            var selectedRange = document.selection.createRange();
            var list = selectedRange.text.split('\n');
            for (i = 0; i < list.length; i++) {
                list[i] = '[li]' + list[i] + '[/li]';
            }
            var str = "";
            do {
                if (list[list.length - 1].toLowerCase() == "[li][/li]")
                    list.pop();
                var str = prompt("Add Item", "");
                if (str != null)
                    list[list.length] = '[li]' + str + '[/li]';
            } while (str);
            if (list[list.length - 1].toLowerCase() == "[li][/li]")
                list.pop();
            selectedRange.text = startTag + '\n' + list.join("\n") + '\n' + endTag;
        }
        else {
            var len = this.text.value.length;
            var start = this.text.selectionStart;
            var end = this.text.selectionEnd;
            var i;
            var scrollTop = this.text.scrollTop;
            var scrollLeft = this.text.scrollLeft;
            var sel = this.text.value.substring(start, end);
            var list = sel.split('\n');
            for (i = 0; i < list.length; i++) {
                list[i] = '[li]' + list[i] + '[/li]';
            }
            var str = "";
            do {
                if (list[list.length - 1].toLowerCase() == "[li][/li]")
                    list.pop();
                var str = prompt("Enter a list item.\n\nLeave the box empty or press 'Cancel' to complete this list:", "");
                if (str != null)
                    list[list.length] = '[li]' + str + '[/li]';
            } while (str);
            if (list[list.length - 1].toLowerCase() == "[li][/li]")
                list.pop();
            var rep = startTag + '\n' + list.join("\n") + '\n' + endTag;
            this.text.value = this.text.value.substring(0, start) + rep + this.text.value.substring(end, len);
            this.text.scrollTop = scrollTop;
            this.text.scrollLeft = scrollLeft;
        }
    };
    return Editor;
})();
//# sourceMappingURL=Editor.js.map