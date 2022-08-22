Shader "Miao/CubeTransition"
{
    Properties
    {
        _MainTex("MainTex",2D) = "White"{}
        backgroudColor("背景颜色", Color) = (0, 0, 0, 1)
        angleX ("起始X轴角度", Range(0, 359)) = 0
        angleY ("相对Y轴角度", Range(0, 359)) = 0
        angleAxis ("主轴角度", Range(0, 359)) = 0
        [IntRange] width ("水平格子数", Range(1,20)) = 1
        [IntRange] height ("垂直格子数", Range(1,20)) = 1
        bar ("滑动条", Range(0, 1)) = 0
        rate ("复原百分比", Range(0.1, 1)) = 0.1
    }
    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            float bar;
            float rate;
            float width;
            float height;
            float angleX;
            float angleY;
            float angleAxis;
            float4 backgroudColor;
            sampler2D _MainTex;
            float4 _MainTex_ST;
            //sampler2D foregroundTexture;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
              	v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;
                //计算单位宽度和高度
                float unitWidth = 1 / width;
                float unitHeight = 1 / height;
                //计算当前UV在整个格子区域中的索引
                int xIndex = floor(uv.x / unitWidth);
                int yIndex = floor(uv.y / unitHeight);
                //缓存x和y增量
                float deltaXRange = unitWidth * 0.5;
                float deltaYRange = unitHeight * 0.5;
                //计算该格子的中心点UV
                float centerUVx = xIndex * unitWidth + deltaXRange;
                float centerUVy = yIndex * unitHeight + deltaYRange;
                //计算最小最大滑动条值
                float minBarValue = deltaXRange + deltaYRange;
                float maxBarValue = (1 - deltaXRange) + (1 - deltaYRange);
                //计算条的可取值范围
                float barValueRange = maxBarValue - minBarValue;
                //计算溢出值
                float overflowValue = barValueRange * rate;
                //计算当前中心的可取值范围
                float currentCenterMinValue = centerUVx + centerUVy;
                float currentCenterMaxValue = currentCenterMinValue + overflowValue;
                float currentCenterValueRange = currentCenterMaxValue - currentCenterMinValue;
                //当前条的值要算上溢出值
                float currentBarValue = minBarValue + (barValueRange + overflowValue) * bar;
                //约束当前条的值,始终映射在[0, 1]区间
                float finalValue = clamp(currentBarValue, currentCenterMinValue, currentCenterMaxValue);
                //映射到[startAngleX, 0]区间
                angleX = angleX - (finalValue - currentCenterMinValue) / currentCenterValueRange * angleX;
                //声明中心UV
                float2 centerUV = float2(centerUVx, centerUVy);
                //根据主轴旋转构建旋转矩阵
                float sinValue;
	            float cosValue;
	            sincos(radians(angleAxis), sinValue, cosValue);
	            float2x2 rotationMatrix = float2x2(cosValue, -sinValue, sinValue, cosValue);
                //计算主轴坐标系下的中心UV
                float2 axisCenterUV = mul(rotationMatrix, centerUV);
                //计算主轴坐标系下的当前片元UV
                float2 axisUV = mul(rotationMatrix, uv);
                //计算在主轴坐标系下的UV距离
                float xDistance = axisUV.x - axisCenterUV.x;
                float yDistance = axisUV.y - axisCenterUV.y;
                //计算经过旋转后投影的新UV
                //原理: 随着角度(弧度)的不断变化,cos的值会进行一系列变化,使得xDistance和yDistance发生变化,进行一个缩放
                float2 newUV = float2(axisCenterUV.x + xDistance / cos(radians(angleX)), axisCenterUV.y +  yDistance / cos(radians(angleY))) * _MainTex_ST.xy + _MainTex_ST.zw;
                //将主轴下的新UV还原回当前坐标系
                newUV = mul(newUV, rotationMatrix);
                //计算当前格子下的各个最小最大值
                float minX = centerUVx - deltaXRange;
                float minY = centerUVy - deltaYRange;
                float maxX = centerUVx + deltaXRange;
                float maxY = centerUVy + deltaYRange;
                //采样前景色
                float4 foregroundColor = tex2D(_MainTex, newUV);
                //计算到底显示后景还是前景
                float isBackgroudX = step(maxX, newUV.x) + step(newUV.x, minX);
                float isBackgroudY = step(maxY, newUV.y) + step(newUV.y, minY);
                float isBackgroud = max(isBackgroudX, isBackgroudY);
                return foregroundColor * (1 - isBackgroud) + backgroudColor * isBackgroud;
            }

            ENDCG
        }
    }
}