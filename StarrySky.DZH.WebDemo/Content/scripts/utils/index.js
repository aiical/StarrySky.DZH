
var both = new Vue({
    el: "#both",
    data: {
        navIndex: '0',
        menuIndex: '1',
        menudata: [
            {
                name: 'demo',
                val: '1111111',
                sub: [{
                    name: 'demoList',
                    val: '#/demo/demoList'
                }]
            },
            {
            name: '客户管理',
            val: '1',
            sub: [{
                name: '客户负责人',
                val: 'a.html'
            }, {
                name: 'KA客户信息',
                val: 'b.html'
            }]
        }, {
            name: '供应商管理',
            val: '2',
            sub: [{
                name: '客户负责人1',
                val: '2-1'
            }, {
                name: 'KA客户信息2',
                val: '2-2'
            }]
        }, {
            name: '客户管理22',
            val: '3',
            sub: [{
                name: '客户负责人22',
                val: '3-1'
            }, {
                name: 'KA客户信息22',
                val: '3-2'
            }]
        }],
        link: './b.html',

    },
    methods: {
        handleNavSelect(key, keyPath) {
            console.log('导航', key, keyPath);
        },
        handleMenuSelect(key, keyPath) {
            this.link = key;
            console.log(this.link)
        }
    }
})
