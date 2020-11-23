/*
 左侧或右侧划出弹框
*/
Vue.component("frame-button", {
    props: {
        child_element: [String, Number],//子元素的元素id，为了让你的元素持续居中
        dialog_id: [String, Number],//弹框id
        dialog_width: {
            type: String,
            default: function () {
                return "900px";
            }
        },//弹框宽度
        dialog_height: {
            type: String,
            default: function () {
                return "650px";
            }
        },//弹框高度
        button_type: {
            default: function () {
                return 'success';
            },
            validator: function (value) {
                // 这个值必须匹配下列字符串中的一个
                return ['success', 'warning', 'danger'].indexOf(value) !== -1
            }
        },//按钮类型
        button_icon: {
            type: String,
            default: function () {
                return "";
            }
        }, //按钮图片
        button_text: {
            type: String,
            required: true
        }, //按钮文字
        button_size: {
            default: function () {
                return 'mini';
            },
            validator: function (value) {
                // 这个值必须匹配下列字符串中的一个
                return ['mini', 'small', 'normal'].indexOf(value) !== -1;
            }
        },//按钮大小
        url: {
            type: String,
            required: true
        },//要嵌入的页面URL
        close_callback: {
            type: String,
            default: function () {
                return "";
            }
        }, //弹框关闭后的回调函数
        title: {
            type: String,
            default: function () {
                return "";
            }//标题
        },
        offset: {
            type: [String, Array],
            default: function () {
                return 'auto';
            }
        },//弹框展示的位置 默认 居中 ,
        anim: {
            type: Number,
            default: function () {
                return 0;
            }
        },//动画效果
        disabled: {
            type: Boolean,
            default: function () {
                return false;
            }
        }//禁用
    },
    computed: {
        minHeight: function () {
            var viewSize = this.$utils.getViewSize();
            var min = this.dialog_height;
            if ((this.dialog_height != undefined && this.dialog_height != "") &&
                (this.dialog_height.indexOf("px") > -1 || this.dialog_height.indexOf("%") == -1)) {
                min = Number(this.dialog_height.replace("px", "")) > viewSize.height
                    ? viewSize.height
                    : Number(this.dialog_height.replace("px", ""));
                min = min + "px";
            } else if (this.dialog_height == undefined || this.dialog_height == "") {
                min = viewSize.height + "px";
            }
            return min;
        },
        minWidth: function () {
            var viewSize = this.$utils.getViewSize();
            var min = this.dialog_width;
            if ((this.dialog_width != undefined && this.dialog_width != "") &&
                (this.dialog_width.indexOf("px") > -1 || this.dialog_width.indexOf("%") == -1)) {
                min = Number(this.dialog_width.replace("px", "")) > viewSize.width
                    ? viewSize.width
                    : Number(this.dialog_width.replace("px", ""));
                min = min + "px";
            } else if (this.dialog_width == undefined || this.dialog_width == "") {
                min = viewSize.width + "px";
            }
            return min;
        }
    },
    template: `<el-button :size="button_size"  :type="button_type" :disabled="disabled"
                            :icon="button_icon"
                            @click='open'>
                            {{button_text}}  
               </el-button>    
                `,
    methods: {
        open: function () {
            if (!this.url) { //没有地址 不弹层
                alert("请指定要打开的页面");
                return false;
            }

            var that = this;
            var index = window.top.layer.open({
                type: 2, //Page层类型
                offset: this.offset || 'r',
                area: [this.minWidth, this.minHeight],
                scrollbar: false,
                title: [this.title, 'font-family:微软雅黑;font-weight:bolder;color:rgb(32, 160, 255)'],
                shade: 0.6 //遮罩透明度
                ,
                maxmin: true //允许全屏最小化
                ,
                anim: this.anim || 3 //0-6的动画形式，-1不开启
                ,
                content: this.url,
                shadeClose: true,
                end: function () { //层销毁回调
                    if (typeof that.close_callback == "function") {
                        that.close_callback();
                    }
                },
                full: function (iframe) {
                    if (!that.child_element) return;
                    var el = $(iframe.context.body).find("[id^='layui-layer-iframe']").contents()
                        .find("#" + that.child_element);
                    el.css({ "margin-right": "auto", "margin-left": "auto" });
                },
                restore: function (iframe) {
                    if (!that.child_element) return;
                    var el = $(iframe.context.body).find("[id^='layui-layer-iframe']").contents()
                        .find("#" + that.child_element);
                    el.css({ "margin-right": "", "margin-left": "" });
                }
            });
        }
    },
    mounted: function () {

    }

});