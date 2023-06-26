﻿// @see http://stackoverflow.com/questions/30716886/es6-read-only-enums-that-can-map-value-to-name
// Immutable Enum maker
function Enum(a) {
    let i = Object
        .keys(a)
        .reduce((o, k) => (o[a[k]] = k, o), {});
    return Object.freeze(
        Object.keys(a).reduce(
            (o, k) => (o[k] = a[k], o), v => i[v]
        )
    );
}

const _lodingType = Enum({
    Div: 0,
    Button: 1,
}),
    _responseType = Enum({
        Success: 0,
        Warning: 1,
        Error: 2
    });

export class Enums {
    static get LoadingType() { return _lodingType; }
    static get ResponseType() { return _responseType; }
}