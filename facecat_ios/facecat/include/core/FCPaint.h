/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu)
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

#ifndef __FCPAINT_H__
#define __FCPAINT_H__
#pragma once
#include "stdafx.h"

namespace FaceCat{
    template <class numtype>
    /*
     * 自定义集合
     */
    class ArrayList{
    public:
        /*
         * 数组
         */
        numtype *m_ary;
        /*
         * 大小
         */
        int m_size;
        /*
         * 容量
         */
        int m_capacity;
        /*
         * 模式
         */
        int m_mode;
        /*
         * 增长步长
         */
        int m_step;
    public:
        /*
         * 构造函数
         */
        ArrayList(){
            m_size = 0;
            m_ary = 0;
            m_capacity = 4;
            m_step = 4;
            m_mode = 0;
        }
        
        /*
         * 构造函数
         * @param capacity 容量
         */
        ArrayList(int capacity){
            m_size = 0;
            m_ary = 0;
            m_capacity = capacity;
            m_step = 4;
            m_mode = 0;
        }
        
        /*
         * 添加
         * @param value 数据
         */
        void add(numtype value){
            m_size += 1;
            if(!m_ary){
                if(m_mode == 0){
                    m_ary = new numtype[m_capacity];
                }
                else{
                    m_ary = (numtype*)malloc(sizeof(numtype) * m_capacity);
                }
            }
            else
            {
                if(m_size > m_capacity){
                    if(m_mode == 0){
                        m_capacity = (int)(m_size * 1.5);
                        numtype *newAry = new numtype[m_capacity];
                        for(int i = 0;i < m_size - 1; i++){
                            newAry[i] = m_ary[i];
                        }
                        delete[] m_ary;
                        m_ary = newAry;
                    }
                    else{
                        m_capacity += m_step;
                        m_ary = (numtype*)realloc(m_ary, sizeof(numtype) * m_capacity);
                    }
                }
            }
            m_ary[m_size - 1] = value;
        }
        
        /*
         * 批量添加
         * @param ary 数据
         * @param size 大小
         */
        void addranges(numtype *ary, int size){
            m_ary = ary;
            m_size = size;
            m_capacity = m_size;
            m_step = 4;
        }
        
        /*
         * 获取容量
         */
        int capacity(){
            return m_capacity;
        }
        
        /*
         * 清除
         */
        void clear(){
            if(m_ary){
                if(m_mode == 0){
                    delete[] m_ary;
                }
                else{
                    free(m_ary);
                }
                m_ary = 0;
            }
            m_size = 0;
        }
        
        /*
         * 获取数据
         */
        numtype get(int index){
            return m_ary[index];
        }
        
        /*
         * 插入数据
         * @param index 索引
         * @param value 数据
         */
        void insert(int index,numtype value){
            m_size += 1;
            if(!m_ary){
                if(m_mode == 0){
                    m_ary = new numtype[m_capacity];
                }
                else{
                    m_ary = (numtype*)malloc(sizeof(numtype) * m_capacity);
                }
            }
            else{
                bool build = false;
                if(m_size > m_capacity){
                    if(m_mode == 0){
                        m_capacity = (int)(m_size * 1.5);
                        numtype *newAry = new numtype[m_capacity];
                        for(int i = 0;i < m_size - 1;i++){
                            if(i < index){
                                newAry[i] = m_ary[i];
                            }
                            else if( i >= index){
                                newAry[i + 1] = m_ary[i];
                            }
                        }
                        delete[] m_ary;
                        m_ary = newAry;
                        build = true;
                    }
                    else{
                        m_capacity += m_step;
                        m_ary = (numtype*)realloc(m_ary, sizeof(numtype) * m_capacity);
                    }
                }
                if(!build)
                {
                    numtype last;
                    for(int i = index; i < m_size; i++){
                        if(i == index){
                            last = m_ary[i];
                        }
                        else if(i > index){
                            numtype temp = m_ary[i];
                            m_ary[i] = last;
                            last = temp;
                        }
                    }
                }
            }
            m_ary[index] = value;
        }
        
        /*
         * 移除数据
         * @param index 索引
         */
        void removeAt(int index){
            m_size -= 1;
            for(int i = index;i < m_size; i++){
                m_ary[i] = m_ary[i + 1];
            }
            bool reduce = false;
            if(m_mode == 0){
                if(m_capacity > 4 && m_size > 0){
                    if(m_capacity > (int)(m_size * 1.5)){
                        m_capacity = (int)(m_size * 1.5);
                        reduce = true;
                    }
                }
            }
            else{
                if(m_capacity - m_size > m_step){
                    m_capacity -= m_step;
                    reduce = true;
                }
            }
            if(reduce){
                if(m_capacity > 0){
                    if(m_mode == 0){
                        numtype *newAry = new numtype[m_capacity];
                        for(int i = 0;i < m_size; i++){
                            newAry[i] = m_ary[i];
                        }
                        delete[] m_ary;
                        m_ary = newAry;
                    }
                    else{
                        m_ary = (numtype*)realloc(m_ary, sizeof(numtype) * m_capacity);
                    }
                }
                else{
                    if(m_ary){
                        if(m_mode == 0){
                            delete[] m_ary;
                        }
                        else{
                            free(m_ary);
                        }
                        m_ary = 0;
                    }
                }
            }
        }
        
        /*
         * 设置数据
         * @param index 索引
         * @param value 值
         */
        void set(int index,numtype value){
            m_ary[index] = value;
        }
        
        /*
         * 设置容量
         * @param capacity 容量
         */
        void setCapacity(int capacity){
            m_capacity = capacity;
            if(m_ary){
                if(m_mode == 0){
                    numtype *newAry = new numtype[m_capacity];
                    for(int i = 0; i < m_size - 1; i++){
                        newAry[i] = m_ary[i];
                    }
                    delete[] m_ary;
                    m_ary = newAry;
                }
                else{
                    m_ary = (numtype*)realloc(m_ary, sizeof(numtype) * m_capacity);
                }
            }
        }
        
        /*
         * 设置步长
         * @param step 步长
         */
        void setStep(int step){
            m_step = step;
        }
        
        /*
         * 获取尺寸
         * @param step 尺寸
         */
        int size(){
            return m_size;
        }
        
        /*
         * 析构函数
         */
        virtual ~ArrayList(){
            clear();
        }
        
        /*
         * 拷贝构造函数
         */
        ArrayList(const ArrayList& rhs){
            if (this != &rhs) {
                m_size = rhs.m_size;
                m_capacity = rhs.m_capacity;
                m_mode = rhs.m_mode;
                m_step = rhs.m_step;
                if(m_mode == 0){
                    m_ary = new numtype[m_capacity];
                }
                else{
                    m_ary = (numtype*)malloc(sizeof(numtype) * m_capacity);
                }
                for (int i = 0; i < m_size; i++){
                    m_ary[i] = rhs.m_ary[i];
                }
            }
        }
        
        /*
         * 拷贝函数
         */
        const ArrayList& operator=(const ArrayList& rhs){
            if (this != &rhs) {
                clear();
                m_size = rhs.m_size;
                m_capacity = rhs.m_capacity;
                m_mode = rhs.m_mode;
                m_step = rhs.m_step;
                if(m_mode == 0){
                    m_ary = new numtype[m_capacity];
                }
                else{
                    m_ary = (numtype*)malloc(sizeof(numtype) * m_capacity);
                }
                for (int i = 0; i < m_size; i++){
                    m_ary[i] = rhs.m_ary[i];
                }
            }
            return *this;
        }
    };
    
    /*
     * 自定义哈希表
     */
    template <class KEY,class VALUE>
    class HashMap{
    private:
        /*
         * 获取char*的哈希
         */
        int hashKey(const char* chPtr){
            int len = (int)strlen(chPtr);
            int hash = 0, offset = 0;
            int h = hash;
            if (h == 0 && len > 0) {
                int off = offset;
                for (int i = 0; i < len; i++) {
                    h = 31 * h + chPtr[off++];
                }
                hash = h;
            }
            return h;
        }
        /*
         * 获取string的哈希
         */
        int hashKey(string key){
            return hashKey(key.c_str());
        }
        /*
         * 获取wstring上的哈希
         */
        int hashKey(String pKey){
#if TARGET_RT_BIG_ENDIAN
            const NSStringEncoding kEncoding_wchar_t = cFStringConvertEncodingToNSStringEncoding(kCFStringEncodingUTF32BE);
#else
            const NSStringEncoding kEncoding_wchar_t = CFStringConvertEncodingToNSStringEncoding(kCFStringEncodingUTF32LE);
#endif
            string sKey;
            @autoreleasepool{
                char* data = (char*)pKey.data();
                unsigned size = pKey.size() * sizeof(wchar_t);
                NSString* result = [[NSString alloc] initWithBytes:data length:size encoding:kEncoding_wchar_t];
                sKey = [result UTF8String];
            }
            const char *chPtr = sKey.c_str();
            int len = (int)strlen(chPtr);
            int hash = 0, offset = 0;
            int h = hash;
            if (h == 0 && len > 0) {
                int off = offset;
                for (int i = 0; i < len; i++) {
                    h = 31 * h + chPtr[off++];
                }
                hash = h;
            }
            return h;
        }
        /*
         * 获取double的哈希
         */
        int hashKey(double key){
            if (key == 0.0){
                return 0;
            }
            long num2 = *((long*) &key);
            return (((int) num2) ^ ((int) (num2 >> 0x20)));
        }
        /*
         * 获取int的哈希
         */
        int hashKey(int key){
            return key;
        }
    protected:
        /*
         * 哈希列表
         */
        ArrayList<int> m_hashs;
        /*
         * 键的列表
         */
        ArrayList<KEY> m_keys;
        /*
         * 值的列表
         */
        ArrayList<VALUE> m_rows;
        /*
         * 添加哈希值
         */
        int addHashCode(int hashCode){
            if(m_hashs.size() == 0 || hashCode > m_hashs.get((int)m_hashs.size() - 1)){
                m_hashs.add(hashCode);
                return m_hashs.size() - 1;
            }
            else{
                int begin = 0;
                int end = m_hashs.size() - 1;
                int sub = end - begin;
                while(sub > 1){
                    int half = begin + sub / 2;
                    int hf = m_hashs.get(half);
                    if(hf > hashCode){
                        end = half;
                    }
                    else if(hf < hashCode){
                        begin = half;
                    }
                    sub = end - begin;
                }
                if(hashCode < m_hashs.get(begin)){
                    m_hashs.insert(begin, hashCode);
                    return begin;
                }
                else if(hashCode > m_hashs.get(end)){
                    m_hashs.insert(end + 1, hashCode);
                    return end + 1;
                }
                else{
                    m_hashs.insert(begin + 1, hashCode);
                    return begin + 1;
                }
            }
            return -1;
        }
    public:
        /*
         * 清除
         */
        void clear(){
            m_hashs.clear();
            m_keys.clear();
            m_rows.clear();
        }
        /*
         * 根据哈希代码查找键的索引
         */
        void findKeyIndexs(int rowIndex, int hashCode, int *list, int *len){
            if(rowIndex != -1){
                int hSize = m_hashs.size();
                int tempIndex = rowIndex;
                while(tempIndex >= 0){
                    if(m_hashs.get(tempIndex) == hashCode){
                        list[*len] = tempIndex;
                        *len = *len + 1;
                    }
                    else{
                        break;
                    }
                    tempIndex--;
                }
                tempIndex = rowIndex + 1;
                while(tempIndex < hSize){
                    if(m_hashs.get(tempIndex) == hashCode){
                        list[*len] = tempIndex;
                        *len = *len + 1;
                    }
                    else{
                        break;
                    }
                    tempIndex++;
                }
            }
        }
        /*
         * 根据哈希代码查找键的多个索引
         */
        int findKeyIndex(int hashCode){
            int low = 0;
            int high = m_hashs.size() - 1;
            while (low <= high) {
                int middle = (low + high) / 2;
                double hf = m_hashs.get(middle);
                if (hashCode == hf) {
                    return middle;
                }
                else if (hashCode > hf) {
                    low = middle + 1;
                }
                else if (hashCode < hf) {
                    high = middle - 1;
                }
            }
            return -1;
        }
        
        void put(KEY key, VALUE value)
        {
            int hashCode = hashKey(key);
            int index = findKeyIndex(hashCode);
            if(index == -1){
                index = addHashCode(hashCode);
            }
            else
            {
                int list[1024];
                int len = 0;
                findKeyIndexs(index, hashCode, list, &len);
                if(len >= 1){
                    index = -1;
                    for(int i = 0; i < len; i++){
                        if(m_keys.get(list[i]) == key){
                            index = list[i];
                            break;
                        }
                    }
                    if(index == -1){
                        index = addHashCode(hashCode);
                    }
                }
            }
            m_keys.insert(index, key);
            m_rows.insert(index, value);
        }
        /*
         * 是否包含键
         */
        bool containsKey(KEY key){
            int hashCode = hashKey(key);
            int index = findKeyIndex(hashCode);
            if(index != -1){
                int list[1024];
                int len = 0;
                findKeyIndexs(index, hashCode, list, &len);
                if(len >= 1){
                    for(int i = 0; i < len; i++) {
                        if(m_keys.get(list[i]) == key){
                            index = list[i];
                            break;
                        }
                    }
                }
                return true;
            }
            return false;
        }
        /*
         * 根据键获取值
         */
        VALUE get(KEY key)
        {
            int hashCode = hashKey(key);
            int index = findKeyIndex(hashCode);
            if(index != -1){
                int list[1024];
                int len = 0;
                findKeyIndexs(index, hashCode, list, &len);
                if(len >= 1){
                    for(int i = 0; i < len; i++){
                        if(m_keys.get(list[i]) == key){
                            index = list[i];
                            break;
                        }
                    }
                }
                return m_rows.get(index);
            }
            return 0;
        }
        /*
         * 根据索引获取键
         */
        KEY getKey(int index){
            return m_keys.get(index);
        }
        /*
         * 根据索引获取值
         */
        VALUE getValue(int index){
            return m_rows.get(index);
        }
        /*
         * 移除
         */
        void remove(KEY key)
        {
            int hashCode = hashKey(key);
            int index = findKeyIndex(hashCode);
            if(index != -1){
                int list[1024];
                int len = 0;
                findKeyIndexs(index, hashCode, list, &len);
                if(len >= 1){
                    index = -1;
                    for(int i = 0; i < len; i++){
                        if(m_keys.get(list[i]) == key){
                            index = list[i];
                            break;
                        }
                    }
                }
                if(index != -1){
                    m_hashs.removeAt(index);
                    m_keys.removeAt(index);
                    m_rows.removeAt(index);
                }
            }
        }
        /*
         * 获取尺寸
         */
        int size(){
            return m_hashs.size();
        }
    public:
        /*
         * 构造函数
         */
        HashMap(){}
        /*
         * 析构函数
         */
        virtual ~HashMap(){clear();}
    public:
        /*
         * 拷贝构造函数
         */
        HashMap(const HashMap& rhs){
            if (this != &rhs) {
                m_hashs = rhs.m_hashs;
                m_keys = rhs.m_keys;
                m_rows = rhs.m_rows;
            }
        }
        /*
         * 重写等于方法
         */
        const HashMap& operator=(const HashMap& rhs){
            if (this != &rhs) {
                clear();
                m_hashs = rhs.m_hashs;
                m_keys = rhs.m_keys;
                m_rows = rhs.m_rows;
            }
            return *this;
        }
    };
    
    struct FCAnchor{
    public:
        /**
         * 底部坐标
         */
        bool bottom;
        /**
         * 左侧坐标
         */
        bool left;
        /**
         * 右侧坐标
         */
        bool right;
        /**
         * 顶部左标
         */
        bool top;
        /**
         * 创建锚定信息
         */
        FCAnchor(){
            bottom = false;
            left = false;
            right = false;
            top = false;
        }
        /**
         * 创建锚定信息
         * @param left  左侧
         * @param top   顶部
         * @param right 右侧
         * @param bottom    底部
         */
        FCAnchor(bool left, bool top, bool right, bool bottom){
            this->left = left;
            this->top = top;
            this->right = right;
            this->bottom = bottom;
        }
    };
    
    /*
     * 控件内容的布局
     */
    enum FCContentAlignment{
        FCContentAlignment_BottomCenter, //中部靠下居中对齐
        FCContentAlignment_BottomLeft, //左下方对齐
        FCContentAlignment_BottomRight, //右下方对齐
        FCContentAlignment_MiddleCenter, //垂直居中
        FCContentAlignment_MiddleLeft, //垂直居中靠左
        FCContentAlignment_MiddleRight, //垂直居中靠右
        FCContentAlignment_TopCenter, //中部靠上居中对齐
        FCContentAlignment_TopLeft, //左上方对齐
        FCContentAlignment_TopRight //右上方对齐
    };
    
    /*
     * 光标
     */
    enum FCCursors{
        FCCursors_Arrow,
        FCCursors_ClosedHand,
        FCCursors_Cross,
        FCCursors_DisappearingItem,
        FCCursors_DragCopy,
        FCCursors_DragLink,
        FCCursors_Hand,
        FCCursors_IBeam,
        FCCursors_IBeamCursorForVerticalLayout,
        FCCursors_No,
        FCCursors_PointingHand,
        FCCursors_SizeDown,
        FCCursors_SizeLeft,
        FCCursors_SizeRight,
        FCCursors_SizeUp,
        FCCursors_SizeWE,
        FCCursors_SizeNS,
        FCCursors_SizeNESW,
        FCCursors_SizeNWSE,
        FCCursors_WaitCursor,
    };
    
    /*
     * 控件绑定边缘类型
     */
    enum FCDockStyle{
        FCDockStyle_Bottom, //底部
        FCDockStyle_Fill, //填充
        FCDockStyle_Left, //左侧
        FCDockStyle_None, //不绑定
        FCDockStyle_Right, //右侧
        FCDockStyle_Top //顶部
    };
    
    /*
     * 控件横向排列方式
     */
    enum FCHorizontalAlign{
        FCHorizontalAlign_Center, //居中
        FCHorizontalAlign_Right, //远离
        FCHorizontalAlign_Inherit, //继承
        FCHorizontalAlign_Left //靠近
    };
    
    /*
     * 控件纵向排列方式
     */
    enum FCVerticalAlign{
        FCVerticalAlign_Bottom, //底部
        FCVerticalAlign_Inherit, //中间
        FCVerticalAlign_Middle, //继承
        FCVerticalAlign_Top //顶部
    };
    
    /*
     * 控件布局样式
     */
    enum FCLayoutStyle{
        FCLayoutStyle_BottomToTop, //自下而上
        FCLayoutStyle_LeftToRight, //从左向右
        FCLayoutStyle_None, //无布局
        FCLayoutStyle_RightToLeft, //从右向左
        FCLayoutStyle_TopToBottom //自上而下
    };
    
    /*
     * 镜像模式
     */
    enum FCMirrorMode{
        FCMirrorMode_BugHole, //虫洞
        FCMirrorMode_None, //无
        FCMirrorMode_Shadow //影子
    };
    
    /**
     * 背景色
     */
    static Long FCColor_Back = (Long)-200000000001;
    /**
     * 边线颜色
     */
    static Long FCColor_Border = (Long)-200000000002;
    /**
     * 前景色
     */
    static Long FCColor_Text = (Long)-200000000003;
    /**
     * 不可用的背景色
     */
    static Long FCColor_DisabledBack = (Long)-200000000004;
    /**
     * 不可用的前景色
     */
    static Long FCColor_DisabledText = (Long)-200000000005;
    /**
     * 触摸悬停的背景色
     */
    static Long FCColor_Hovered = (Long)-200000000006;
    /**
     * 触摸被按下的背景色
     */
    static Long FCColor_Pushed = (Long)-200000000007;
    /**
     * 空颜色
     */
    static Long FCColor_None = (Long)-200000000000;
    
    class FCPaint;
    
    /*
     * 颜色表示
     */
    class FCColor{
    public:
        /*
         * 获取argb值
         */
        static Long argb(int r, int g, int b);
        /**
         * 获取RGB颜色
         * @param r 红色值
         * @param g 绿色值
         * @param b 蓝色值
         * @returns RGB颜色
         */
        static Long argb(int a, int r, int g, int b);
        /**
         * 获取RGB颜色
         * @param a  透明值
         */
        static void toArgb(FCPaint *paint, Long dwPenColor, int *a, int *r, int *g, int *b);
        /**
         * 获取比例色
         * @param originalColor  原始色
         * @param ratio  比例
         */
        static Long ratioColor(FCPaint *paint, Long originalColor, double ratio);
        /**
         * 获取反色
         */
        static Long reverse(FCPaint *paint, Long originalColor);
    };
    
    /*
     * 坐标点
     */
    struct FCPoint{
        /**
         * 创建浮点
         */
    public:
        int x;
        int y;
    };
    
    /*
     * 坐标点
     */
    struct FCPointF{
        /**
         * 创建浮点
         */
    public:
        float x;
        float y;
    };
    
    /*
     * 尺寸
     */
    struct FCSize{
        /**
         * 创建浮点型尺寸
         */
    public:
        int cx;
        int cy;
    };
    
    /*
     * 尺寸
     */
    struct FCSizeF{
        /**
         * 创建浮点型尺寸
         */
    public:
        float cx;
        float cy;
    };
    
    /*
     * 矩形
     */
    struct FCRect{
        /**
         * 创建矩形
         */
    public:
        /**
         * 左侧坐标
         */
        int left;
        /**
         * 顶部坐标
         */
        int top;
        /**
         * 右侧坐标
         */
        int right;
        /**
         * 底部坐标
         */
        int bottom;
    };
    
    /*
     * 矩形
     */
    struct FCRectF{
        /**
         * 创建矩形
         */
    public:
        /**
         * 左侧坐标
         */
        float left;
        /**
         * 顶部坐标
         */
        float top;
        /**
         * 右侧坐标
         */
        float right;
        /**
         * 底部坐标
         */
        float bottom;
    };
    
    /*
     * 字体
     */
    class FCFont{
    public:
        /**
         * 字体
         */
        String m_fontFamily;
        /**
         * 字体大小
         */
        float m_fontSize;
        /**
         * 是否粗体
         */
        bool m_bold;
        /**
         * 是否有下划线
         */
        bool m_underline;
        /**
         * 是否斜体
         */
        bool m_italic;
        /**
         * 是否有删除线
         */
        bool m_strikeout;
        /*
         * 创建字体
         */
        FCFont(){
            m_fontFamily = L"Simsun";
            m_fontSize = 12;
            m_bold = false;
            m_underline = false;
            m_italic = false;
            m_strikeout = false;
        }
        /**
         * 创建字体
         * @param  fontFamily 字体
         * @param fontSize  字号
         * @param bold 是否粗体
         * @param underline 是否有下划线
         * @param italic 是否斜体
         * @param strikeout 是否有删除线
         */
        FCFont(const String& fontFamily, float fontSize, bool bold, bool underline, bool italic){
            m_fontFamily = fontFamily;
            m_fontSize = fontSize;
            m_bold = bold;
            m_underline = underline;
            m_italic = italic;
            m_strikeout = false;
        }
        /**
         * 创建字体
         * @param  fontFamily 字体
         * @param fontSize  字号
         * @param bold 是否粗体
         * @param underline 是否有下划线
         * @param italic 是否斜体
         * @param strikeout 是否有删除线
         */
        FCFont(const String& fontFamily, float fontSize, bool bold, bool underline, bool italic, bool strikeout){
            m_fontFamily = fontFamily;
            m_fontSize = fontSize;
            m_bold = bold;
            m_underline = underline;
            m_italic = italic;
            m_strikeout = strikeout;
        }
    public:
        /*
         * 拷贝字体
         */
        void copy(FCFont *font){
            m_fontFamily = font->m_fontFamily;
            m_fontSize = font->m_fontSize;
            m_bold = font->m_bold;
            m_underline = font->m_underline;
            m_italic = font->m_italic;
            m_strikeout = font->m_strikeout;
        }
    };
    
    /*
     * 边距
     */
    struct FCPadding{
    public:
        /**
         * 底边距
         */
        int bottom;
        /**
         * 左边距
         */
        int left;
        /**
         * 右边距
         */
        int right;
        /**
         * 顶边距
         */
        int top;
        /**
         * 创建边距
         */
        FCPadding(){
            bottom = 0;
            left = 0;
            right = 0;
            top = 0;
        }
        /**
         * 创建边距
         */
        FCPadding(int all){
            bottom = all;
            left = all;
            right = all;
            top = all;
        }
        /**
         * 创建边距
         */
        FCPadding(int left, int top, int right, int bottom){
            this->left = left;
            this->top = top;
            this->right = right;
            this->bottom = bottom;
        }
    };
    
    /*
     * 触摸信息
     */
    class FCTouchInfo{
    public:
        /*
         * 点击次数
         */
        int m_clicks;
        /**
         * 滚动值
         */
        int m_delta;
        /**
         * 是否第一个坐标
         */
        bool m_firstTouch;
        /**
         * 第一个坐标
         */
        FCPoint m_firstPoint;
        /**
         * 是否第二个坐标
         */
        bool m_secondTouch;
        FCPoint m_secondPoint;
    public:
        /*
         * 构造函数
         */
        FCTouchInfo(){
            m_clicks = 0;
            m_delta = 0;
            m_firstTouch = false;
            m_firstPoint.x = 0;
            m_firstPoint.y = 0;
            m_secondTouch = false;
            m_secondPoint.x = 0;
            m_secondPoint.y = 0;
        }
        /*
         * 析构函数
         */
        ~FCTouchInfo(){
        }
    };
    
    /*
     * 绘图
     */
    class FCPaint{
    public:
        /*
         * 构造函数
         */
        FCPaint();
        /*
         * 析构函数
         */
        virtual ~FCPaint();
    public:
        /**
         * 添加曲线
         * @param  rect 矩形区域
         * @param startAngle 从 x 轴到弧线的起始点沿顺时针方向度量的角（以度为单位）
         * @param sweepAngle 从 startAngle 参数到弧线的结束点沿顺时针方向度量的角（以度为单位）
         */
        virtual void addArc(const FCRect& rect, float startAngle, float sweepAngle);
        /**
         * 添加贝赛尔曲线
         * @param  point1  坐标1
         * @param  point2  坐标2
         * @param  point3  坐标3
         * @param  point4  坐标4
         */
        virtual void addBezier(FCPoint *apt, int cpt);
        /**
         * 添加曲线
         * @param  points  点阵
         */
        virtual void addCurve(FCPoint *apt, int cpt);
        /**
         * 添加椭圆
         * @param  rect 矩形
         */
        virtual void addEllipse(const FCRect& rect);
        /**
         * 添加直线
         * @param  x1 第一个点的横坐标
         * @param  y1 第一个点的纵坐标
         * @param  x2 第二个点的横坐标
         * @param  y2 第二个点的纵坐标
         */
        virtual void addLine(int x1, int y1, int x2, int y2);
        /**
         * 添加矩形
         * @param  rect 区域
         */
        virtual void addRect(const FCRect& rect);
        /**
         * 添加扇形
         * @param  rect 矩形区域
         * @param startAngle 从 x 轴到弧线的起始点沿顺时针方向度量的角（以度为单位）
         * @param sweepAngle 从 startAngle 参数到弧线的结束点沿顺时针方向度量的角（以度为单位）
         */
        virtual void addPie(const FCRect& rect, float startAngle, float sweepAngle);
        /**
         * 添加文字
         * @param  text 文字
         * @param  font 字体
         * @param  rect 区域
         */
        virtual void addText(const wchar_t *strText, FCFont *font, const FCRect& rect);
        /**
         * 开始绘图
         * @param hdc  HDC
         * @param wRect 窗体区域
         * @param pRect 刷新区域
         */
        virtual void beginPaint(int hDC, const FCRect& wRect, const FCRect& pRect);
        /**
         * 开始一段路径
         */
        virtual void beginPath();
        /**
         * 清除缓存
         */
        virtual void clearCaches();
        /**
         * 裁剪路径
         */
        virtual void clipPath();
        /**
         * 闭合路径
         */
        virtual void closeFigure();
        /**
         * 结束一段路径
         */
        virtual void closePath();
        /**
         * 绘制弧线
         * @param  dwPenColor 颜色
         * @param  width  宽度
         * @param  style  样式
         * @param  rect   矩形区域
         * @param  startAngle  从 x 轴到弧线的起始点沿顺时针方向度量的角（以度为单位）
         * @param sweepAngle   从 startAngle 参数到弧线的结束点沿顺时针方向度量的角（以度为单位）
         */
        virtual void drawArc(Long dwPenColor, float width, int style, const FCRect& rect, float startAngle, float sweepAngle);
        /**
         * 设置贝赛尔曲线
         * @param  dwPenColor 颜色
         * @param  width  宽度
         * @param  style  样式
         * @param points  坐标阵
         */
        virtual void drawBezier(Long dwPenColor, float width, int style, FCPoint *apt, int cpt);
        /**
         * 绘制曲线
         * @param  dwPenColor 颜色
         * @param  width  宽度
         * @param  style  样式
         * @param points  坐标阵
         */
        virtual void drawCurve(Long dwPenColor, float width, int style, FCPoint *apt, int cpt);
        /**
         * 绘制矩形
         * @param  dwPenColor 颜色
         * @param  width  宽度
         * @param  style  样式
         * @param  rect   矩形区域
         */
        virtual void drawEllipse(Long dwPenColor, float width, int style, const FCRect& rect);
        /**
         * 绘制矩形
         * @param  dwPenColor 颜色
         * @param  width  宽度
         * @param  style  样式
         * @param  left 左侧坐标
         * @param  top  顶部左标
         * @param  right 右侧坐标
         * @param  bottom  底部坐标
         */
        virtual void drawEllipse(Long dwPenColor, float width, int style, int left, int top, int right, int bottom);
        /**
         * 绘制图片
         * @param  imagePath  图片路径
         * @param  rect   绘制区域
         */
        virtual void drawImage(const wchar_t *imagePath, const FCRect& rect);
        /**
         * 绘制直线
         * @param  dwPenColor 颜色
         * @param  width  宽度
         * @param  style  样式
         * @param  x1 第一个点的横坐标
         * @param  y1 第一个点的纵坐标
         * @param  x2 第二个点的横坐标
         * @param  y2 第二个点的纵坐标
         */
        virtual void drawLine(Long dwPenColor, float width, int style, const FCPoint& x, const FCPoint& y);
        /**
         * 绘制直线
         * @param  dwPenColor 颜色
         * @param  width  宽度
         * @param  style  样式
         * @param  x 第一个点坐标
         * @param  y 第二个点的坐标
         */
        virtual void drawLine(Long dwPenColor, float width, int style, int x1, int y1, int x2, int y2);
        /**
         * 绘制路径
         * @param  dwPenColor 颜色
         * @param  width  宽度
         * @param  style  样式
         */
        virtual void drawPath(Long dwPenColor, float width, int style);
        /**
         * 绘制弧线
         * @param  dwPenColor 颜色
         * @param  width  宽度
         * @param  style  样式
         * @param  rect   矩形区域
         * @param  startAngle  从 x 轴到弧线的起始点沿顺时针方向度量的角（以度为单位）
         * @param sweepAngle   从 startAngle 参数到弧线的结束点沿顺时针方向度量的角（以度为单位）
         */
        virtual void drawPie(Long dwPenColor, float width, int style, const FCRect& rect, float startAngle, float sweepAngle);
        /**
         * 绘制多边形
         * @param  dwPenColor 颜色
         * @param  width  宽度
         * @param  style  样式
         * @param  points  点的数组
         */
        virtual void drawPolygon(Long dwPenColor, float width, int style, FCPoint *apt, int cpt);
        /**
         * 绘制大量直线
         * @param  dwPenColor 颜色
         * @param  width  宽度
         * @param  style  样式
         * @param  points  点集
         */
        virtual void drawPolyline(Long dwPenColor, float width, int style, FCPoint *apt, int cpt);
        /**
         * 绘制矩形
         * @param  dwPenColor 颜色
         * @param  width  宽度
         * @param  style  样式
         * @param  rect   矩形区域
         */
        virtual void drawRect(Long dwPenColor, float width, int style, int left, int top, int right, int bottom);
        /**
         * 绘制矩形
         * @param  dwPenColor 颜色
         * @param  width  宽度
         * @param  style  样式
         * @param  left 左侧坐标
         * @param  top  顶部左标
         * @param  right 右侧坐标
         * @param  bottom  底部坐标
         */
        virtual void drawRect(Long dwPenColor, float width, int style, const FCRect& rect);
        /**
         * 绘制圆角矩形
         * @param  dwPenColor 颜色
         * @param  width  宽度
         * @param  style  样式
         * @param  rect   矩形区域
         * @param  cornerRadius 边角半径
         */
        virtual void drawRoundRect(Long dwPenColor, float width, int style, const FCRect& rect, int cornerRadius);
        /**
         * 绘制矩形
         * @param  text   文字
         * @param  dwPenColor 颜色
         * @param  font   字体
         * @param  rect   矩形区域
         */
        virtual void drawText(const wchar_t *strText, Long dwPenColor, FCFont *font, const FCRect& rect);
        /**
         * 绘制矩形
         * @param  text   文字
         * @param  dwPenColor 颜色
         * @param  font   字体
         * @param  rect   矩形区域
         */
        virtual void drawText(const wchar_t *strText, Long dwPenColor, FCFont *font, const FCRectF& rect);
        /**
         * 绘制自动省略结尾的文字
         * @param  text   文字
         * @param  dwPenColor 颜色
         * @param  font   字体
         * @param  rect   矩形区域
         */
        virtual void drawTextAutoEllipsis(const wchar_t *strText, Long dwPenColor, FCFont *font, const FCRect& rect);
        /**
         * 结束绘图
         */
        virtual void endPaint();
        /*
         * 去除裁剪路径
         */
        virtual void excludeClipPath();
        /**
         * 填充椭圆
         * @param  dwPenColor 颜色
         * @param  rect   矩形区域
         */
        virtual void fillEllipse(Long dwPenColor, const FCRect& rect);
        /**
         * 绘制渐变椭圆
         * @param  dwFirst  开始颜色
         * @param  dwSecond 结束颜色
         * @param  rect     矩形
         * @param  angle    角度
         */
        virtual void fillGradientEllipse(Long dwFirst, Long dwSecond, const FCRect& rect, int angle);
        /*
         * 绘制渐变区域
         */
        virtual void fillGradientPath(Long dwFirst, Long dwSecond, const FCRect& rect, int angle);
        /**
         * 填充渐变路径
         * @param  dwFirst  开始颜色
         * @param  dwSecond 结束颜色
         * @param  points   点的集合
         * @param  angle    角度
         */
        virtual void fillGradientPolygon(Long dwFirst, Long dwSecond, FCPoint *apt, int cpt, int angle);
        /**
         * 绘制渐变矩形
         * @param  dwFirst  开始颜色
         * @param  dwSecond 结束颜色
         * @param  rect     矩形
         * @param  cornerRadius     圆角半径
         * @param  angle    角度
         */
        virtual void fillGradientRect(Long dwFirst, Long dwSecond, const FCRect& rect, int cornerRadius, int angle);
        /**
         * 填充路径
         * @param  dwPenColor 颜色
         */
        virtual void fillPath(Long dwPenColor);
        /**
         * 绘制扇形
         * @param  dwPenColor 颜色
         * @param  rect   矩形区域
         * @param  startAngle  从 x 轴到弧线的起始点沿顺时针方向度量的角（以度为单位）
         * @param sweepAngle   从 startAngle 参数到弧线的结束点沿顺时针方向度量的角（以度为单位）
         */
        virtual void fillPie(Long dwPenColor, const FCRect& rect, float startAngle, float sweepAngle);
        /**
         * 填充椭圆
         * @param  dwPenColor 颜色
         * @param  points     点的数组
         */
        virtual void fillPolygon(Long dwPenColor, FCPoint *apt, int cpt);
        /**
         * 填充矩形
         * @param  dwPenColor 颜色
         * @param  rect   矩形区域
         */
        virtual void fillRect(Long dwPenColor, const FCRect& rect);
        /**
         * 填充矩形
         * @param  text   文字
         * @param  dwPenColor 颜色
         * @param  font   字体
         * @param  rect   矩形区域
         */
        virtual void fillRect(Long dwPenColor, int left, int top, int right, int bottom);
        /**
         * 填充圆角矩形
         * @param  dwPenColor 颜色
         * @param  rect   矩形区域
         * @param  cornerRadius 边角半径
         */
        virtual void fillRoundRect(Long dwPenColor, const FCRect& rect, int cornerRadius);
        /**
         * 获取颜色
         * @param  dwPenColor 输入颜色
         * @returns   输出颜色
         */
        virtual Long getColor(Long dwPenColor);
        /**
         * 获取要绘制的颜色
         * @param  dwPenColor 输入颜色
         * @returns   输出颜色
         */
        virtual Long getPaintColor(Long dwPenColor);
        /**
         * 获取偏移
         */
        virtual FCPoint getOffset();
        /**
         * 旋转角度
         * @param  op   圆心坐标
         * @param  mp   点的坐标
         * @param  angle  角度
         * @returns  结果坐标
         */
        virtual FCPoint rotate(const FCPoint& op, const FCPoint& mp, int angle);
        /**
         * 设置裁剪区域
         * @param  rect   区域
         */
        virtual void setClip(const FCRect& rect);
        /**
         * 设置直线两端的样式
         * @param  startLineCap  开始的样式
         * @param  endLineCap  结束的样式
         */
        virtual void setLineCap(int startLineCap, int endLineCap);
        /**
         * 设置偏移
         * @param  mp  偏移坐标
         */
        virtual void setOffset(const FCPoint& offset);
        /**
         * 设置透明度
         * @param  opacity  透明度
         */
        virtual void setOpacity(float opacity);
        /**
         * 设置资源的路径
         * @param  resourcePath  资源的路径
         */
        virtual void setResourcePath(const String& resourcePath);
        /**
         * 设置旋转角度
         * @param  rotateAngle  旋转角度
         */
        virtual void setRotateAngle(int rotateAngle);
        /**
         * 设置缩放因子
         * @param  scaleFactorX   横向因子
         * @param  scaleFactorY   纵向因子
         */
        virtual void setScaleFactor(double scaleFactorX, double scaleFactorY);
        /**
         * 设置平滑模式
         * @param  smoothMode  平滑模式
         */
        virtual void setSmoothMode(int smoothMode);
        /**
         * 设置文字的质量
         * @param  textQuality  文字质量
         */
        virtual void setTextQuality(int textQuality);
        /**
         * 设置是否支持透明色
         * @returns  是否支持
         */
        virtual bool supportTransparent();
        /**
         * 获取文字大小
         * @param  text   文字
         * @param  font   字体
         * @returns  字体大小
         */
        virtual FCSize textSize(const wchar_t *strText, FCFont *font);
        /**
         * 获取文字大小
         * @param  text   文字
         * @param  font   字体
         * @returns  字体大小
         */
        virtual FCSizeF textSizeF(const wchar_t *strText, FCFont *font);
    };
}

#endif
