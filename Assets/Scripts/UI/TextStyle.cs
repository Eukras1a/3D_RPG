using TMPro;
using UnityEngine;

public class TextStyle
{
    /// <summary>
    /// 文字的起始大小
    /// </summary>
    public float fontStartSize;
    /// <summary>
    /// 文字的最终大小
    /// </summary>
    public float fontFinalSize;
    /// <summary>
    /// 文字从起始大小变化到最终大小需要的时间
    /// </summary>
    public float fontSizeScaleTime;
    /// <summary>
    /// 文字的颜色
    /// </summary>
    public Color fontColor;
    /// <summary>
    /// 对齐方式
    ///  Left左对齐，Right右对齐，Center居中对齐（推荐使用Geometry代替,居的更中)，Flush文字挤在中间。
    /// </summary>
    public HorizontalAlignmentOptions alignment;
    /// <summary>
    /// 生命周期
    /// </summary>
    public float lifeTime;
    /// <summary>
    /// 文字可以活动的时间，超过此时间后文字将固定，不受力量影响。
    /// </summary>
    public float animateTime;
    /// <summary>
    /// 多久后文字开始透明直至消失。
    /// </summary>
    public float startFadeTime;
    /// <summary>
    /// 最小初始力量，系统会在最大和最小初始力量之间随机一个数值，设为该次文字显示的力量
    /// </summary>
    public Vector3 minVelocity;
    /// <summary>
    /// 最大初始力量，系统会在最大和最小初始力量之间随机一个数值，设为该次文字显示的力量
    /// </summary>
    public Vector3 maxVelocity;
    /// <summary>
    /// 如果有文字受到重力影响的需求，请修改此项。
    /// </summary>
    public Vector3 gravity;
    /// <param name="fontStartSize">文字的起始大小</param>
    /// <param name="fontFinalSize">文字的最终大小</param>
    /// <param name="fontSizeScaleTime">文字从起始大小变化到最终大小需要的时间</param>
    /// <param name="fontColor">文字的颜色</param>
    /// <param name="alignment">对齐方式</param>
    /// <param name="lifeTime">生命周期</param>
    /// <param name="animateTime">文字可以活动的时间，超过此时间后文字将固定，不受力量影响。</param>
    /// <param name="startFadeTime">多久后文字开始透明直至消失。</param>
    /// <param name="minVelocity">最小初始力量，系统会在最大和最小初始力量之间随机一个数值，设为该次文字显示的力量</param>
    /// <param name="maxVelocity">最大初始力量，系统会在最大和最小初始力量之间随机一个数值，设为该次文字显示的力量</param>
    /// <param name="gravity">如果有文字受到重力影响的需求，请修改此项。</param>
    public TextStyle(float fontStartSize, float fontFinalSize, float fontSizeScaleTime, Color fontColor, HorizontalAlignmentOptions alignment, float lifeTime, float animateTime, float startFadeTime, Vector3 minVelocity, Vector3 maxVelocity, Vector3 gravity)
    {
        this.fontStartSize = fontStartSize;
        this.fontFinalSize = fontFinalSize;
        this.fontSizeScaleTime = fontSizeScaleTime;
        this.fontColor = fontColor;
        this.alignment = alignment;
        this.lifeTime = lifeTime;
        this.animateTime = animateTime;
        this.startFadeTime = startFadeTime;
        this.minVelocity = minVelocity;
        this.maxVelocity = maxVelocity;
        this.gravity = gravity;
    }
}
