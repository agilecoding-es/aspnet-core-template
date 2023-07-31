import { Enums } from './enums'

export function displayType(element) {
    var cStyle = element.currentStyle || window.getComputedStyle(element, "");
    if (cStyle.display == 'inline')
        return Enums.DisplayType.Inline;
    else
        return Enums.DisplayType.Block;
}

