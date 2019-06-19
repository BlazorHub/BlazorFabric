//declare interface Window { debounce(func: Function, wait: number, immediate: boolean): Function }
var BlazorFabricContextualMenu;
(function (BlazorFabricContextualMenu) {
    function registerHandlers(targetElement, contextualMenuItem) {
        var window = targetElement.ownerDocument.defaultView;
        var mouseEnterId = Handler.addListener(targetElement, "mouseenter", function (ev) { ev.preventDefault(); contextualMenuItem.invokeMethodAsync("MouseEnterHandler"); }, false);
        var mouseLeaveId = Handler.addListener(targetElement, "mouseleave", function (ev) { ev.preventDefault(); contextualMenuItem.invokeMethodAsync("MouseLeaveHandler"); }, false);
        return [mouseEnterId, mouseLeaveId];
    }
    BlazorFabricContextualMenu.registerHandlers = registerHandlers;
    function unregisterHandlers(ids) {
        for (var _i = 0, ids_1 = ids; _i < ids_1.length; _i++) {
            var id = ids_1[_i];
            Handler.removeListener(id);
        }
    }
    BlazorFabricContextualMenu.unregisterHandlers = unregisterHandlers;
    var Handler = /** @class */ (function () {
        function Handler() {
        }
        Handler.addListener = function (element, event, handler, capture) {
            element.addEventListener(event, handler, capture);
            this.listeners[this.i] = { capture: capture, event: event, handler: handler, element: element };
            return this.i++;
        };
        Handler.removeListener = function (id) {
            if (id in this.listeners) {
                var h = this.listeners[id];
                h.element.removeEventListener(h.event, h.handler, h.capture);
                delete this.listeners[id];
            }
        };
        Handler.i = 1;
        Handler.listeners = {};
        return Handler;
    }());
})(BlazorFabricContextualMenu || (BlazorFabricContextualMenu = {}));
window['BlazorFabricContextualMenu'] = BlazorFabricContextualMenu || {};
//# sourceMappingURL=contextualMenu.js.map