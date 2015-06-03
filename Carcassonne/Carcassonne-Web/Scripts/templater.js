// Templater.js
// A simple template engine made by Wannes Vandenbulcke

var Templater = function (data, template) {

    var createTag = function (tagname, value, attributes) {
        var tag = '<' + tagname;
        var endsOnFirstTagNames = ['input', 'img'];

        var endsOnFirst = endsOnFirstTagNames.indexOf(tagname) > -1

        if (attributes) {
            if (endsOnFirst && value) { attributes["value"] = value; }
            for (var prop in attributes) {
                tag += ' ' + prop + '="' + attributes[prop] + '" ';
            }
        }

        return tag + (endsOnFirst ? '/>' : '>' + value + '</' + tagname + '>');
    };

    var findMatchesLevel = function (teststring) {

        var stringmatches = [];
        var level = 0;
        var fnd = false;
        var str = "";

        for (var i = 1; i < teststring.length - 1; i++) {
            if (teststring[i - 1] == '{') {
                level += fnd ? 1 : 0;
                fnd = true;
            }
            str += fnd ? teststring[i] : "";
            if (teststring[i + 1] == '}') {
                if (level === 0) {
                    fnd = false;
                    stringmatches.push(str);
                    str = "";
                } else {
                    level--;
                }
            }
        }
        return stringmatches;
    };

    var isObject = function (obj) {
        return obj === Object(obj);
    }

    var matches = findMatchesLevel(template); 		//Find matches on first level
    for (var i in matches) {		//Iterate over every found pattern over the first level
        var opts = matches[i].split(":");			//Split every pattern
        var el = "";

        if (data[opts[0]] && (data[opts[0]].constructor === Array || isObject(data[opts[0]]))) {		//Check if the data object is an array
            if (opts[1]) {                 //If the second part of the pattern is empty we assume the length of the array we need to return
                if (data[opts[0]].constructor != Array) {
                    el += Templater(data[opts[0]], /:(.*):?/gi.exec(matches[i])[1]);
                } else {
                    for (var item in data[opts[0]]) {
                        el += Templater(data[opts[0]][item], /:(.*):?/gi.exec(matches[i])[1]);	//Recursvely call Templater function for every nested pattern
                    }
                }
            } else if (!opts[2]) {
                el = data[opts[0]].length + opts[3];
            } else {
                el = createTag(opts[2], opts[3].replace('%1', data[opts[0]].length));
            }
        } else {
            el = opts ? (!!data[opts[0]] || data[opts[0]] === 0 ? data[opts[0]] : "") : "";	//Initially set element to first part of the pattern

            if (opts.length > 1) {

                var attributes = { id: opts[1] };

                if (opts.length >= 3) { attributes["class"] = opts[2]; } //If it has 3 or more elements, we set the class
                if (opts[0] === 'img') { attributes["src"] = data[opts[1]]; }
                if (opts[0] === 'a') { attributes["href"] = data[opts[1]]; }
                if (opts[0] === 'input') { attributes["placeholder"] = data[opts[1]]; attributes["name"] = data[opts[1]]; }

                if (opts[3]) {
                    el = createTag(opts[0], opts[3].replace('%1', data[opts[1]]), attributes);
                } else {
                    el = createTag(opts[0], data[opts[1]], attributes);
                }
            }
        }
        template = template.replace('{' + matches[i] + '}', el);		//Replace every match with the templated string
    }
    return template;
}



