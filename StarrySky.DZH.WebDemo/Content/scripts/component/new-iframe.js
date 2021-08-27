/*
 iframe 控件
  注：
     is_set_height_by_view：是否通过可视区域的高度来给iframe赋值
                       否则是根据iframe里的内容计算iframe高度
     height_adjustment:如果设置 is_set_height_by_view为true 则此变量表示调整的高度差 默认200
     url：表示嵌入的页面地址
 */
Vue.component("new-iframe", {
    props: ["url", "is_set_height_by_view", "height_adjustment", "lazy_url"],
    template: ` <div>
                        <div ref="loading" style="width: 100%;height: 100%;"></div>
                        <iframe ref="iframe"  frameborder=0 
                         width='100%' 
                         :scrolling="getNeedScrolling()" 
                         :src='url'
                         @load='reCaculateWithHeight'  /></div>
                </div> 
              `,
    data: function () {
        return {
            intervalAutoFiller: undefined//自适应计时器
        }
    },
    watch: {},
    methods: {
        loadUrl: function () {
            if (!this.$refs.iframe.src)  this.$refs.iframe.src = this.lazy_url;
        },
        getNeedScrolling: function () {
            return this.is_set_height_by_view == "true" ? "auto" : "no";
        },
        showLoader() {
            var viewSize = this.$utils.getViewSize();
            $(this.$refs.loading).height((viewSize.height || 500) - 200 + 'px');//loding 展示区域设定高度
            this.$utils.loading.show.call(this, "正在为您加载页面。。。", this.$refs.loading)
        },
        closeLoader() {
            $(this.$refs.loading).hide();
            this.$utils.loading.hide.call(this)
        },
        reCaculateWithHeight() {
            this.closeLoader(); //关闭加载中
            var iframe = this.$refs.iframe;
            if (this.is_set_height_by_view == "true") {   //如果设定了高度，则直接return掉 不需要计算高度 
                $(iframe).css("overflow-x", "hidden");
                var viewSize = this.$utils.getViewSize();
                iframe.height = viewSize.height - parseFloat(this.height_adjustment || 200);
            }
            else {
                setTimeout(function () {
                    clearInterval(this.intervalAutoFiller)
                }, 10000);//10秒钟后 取消重新计算高度方法的调用

                this.intervalAutoFiller = setInterval(() => {
                    try {
                        iframe.height = this.getIframeHeight(iframe);
                    } catch (e) {
                        if (e.toString().indexOf("cross-origin") >= 0) {
                            //跨域异常 
                            $(iframe).parent().addClass("embed-responsive embed-responsive-4by3")
                            $(iframe).addClass("embed-responsive-item");
                            clearInterval(this.intervalAutoFiller);
                        }
                    }
                }, 100);
            }
        },
        //获取iframe高度
        getIframeHeight(iframe) {

            var height1 = 0;
            var height2 = 0;
            if (!iframe.contentWindow) return;
            if (iframe.contentWindow.document && iframe.contentWindow.document.body) {
                height1 = iframe.contentWindow.document.body.scrollHeight || 0;
            }
            if (iframe.contentWindow.document.documentElement) {
                height2 = iframe.contentWindow.document.documentElement.scrollHeight || 0;
            }
            return Math.max(height1, height2); // 取最大值

        },
        //获取iframe宽度
        getIframeWidth(iframe) {
            var width1 = 0;
            var width2 = 0;
            if (iframe.contentWindow.document.body) {
                width1 = iframe.contentWindow.document.body.scrollWidth || 0;
            }
            if (iframe.contentWindow.document.documentElement) {
                width2 = iframe.contentWindow.document.documentElement.scrollWidth || 0;
            }
            return Math.max(width1, width2); // 取最大值
        }
    },
    mounted: function () {
        if (this.url) this.showLoader();
    }
})


