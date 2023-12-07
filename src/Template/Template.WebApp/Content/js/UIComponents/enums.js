// @see http://stackoverflow.com/questions/30716886/es6-read-only-enums-that-can-map-value-to-name
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

const _responseType = Enum({
        Success: 0,
        Info: 1,
        Validation: 2,
        Error: 3
    });

export class Enums {
    static get ResponseType() { return _responseType; }
}