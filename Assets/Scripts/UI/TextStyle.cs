using TMPro;
using UnityEngine;

public class TextStyle
{
    /// <summary>
    /// ���ֵ���ʼ��С
    /// </summary>
    public float fontStartSize;
    /// <summary>
    /// ���ֵ����մ�С
    /// </summary>
    public float fontFinalSize;
    /// <summary>
    /// ���ִ���ʼ��С�仯�����մ�С��Ҫ��ʱ��
    /// </summary>
    public float fontSizeScaleTime;
    /// <summary>
    /// ���ֵ���ɫ
    /// </summary>
    public Color fontColor;
    /// <summary>
    /// ���뷽ʽ
    ///  Left����룬Right�Ҷ��룬Center���ж��루�Ƽ�ʹ��Geometry����,�ӵĸ���)��Flush���ּ����м䡣
    /// </summary>
    public HorizontalAlignmentOptions alignment;
    /// <summary>
    /// ��������
    /// </summary>
    public float lifeTime;
    /// <summary>
    /// ���ֿ��Ի��ʱ�䣬������ʱ������ֽ��̶�����������Ӱ�졣
    /// </summary>
    public float animateTime;
    /// <summary>
    /// ��ú����ֿ�ʼ͸��ֱ����ʧ��
    /// </summary>
    public float startFadeTime;
    /// <summary>
    /// ��С��ʼ������ϵͳ����������С��ʼ����֮�����һ����ֵ����Ϊ�ô�������ʾ������
    /// </summary>
    public Vector3 minVelocity;
    /// <summary>
    /// ����ʼ������ϵͳ����������С��ʼ����֮�����һ����ֵ����Ϊ�ô�������ʾ������
    /// </summary>
    public Vector3 maxVelocity;
    /// <summary>
    /// ����������ܵ�����Ӱ����������޸Ĵ��
    /// </summary>
    public Vector3 gravity;
    /// <param name="fontStartSize">���ֵ���ʼ��С</param>
    /// <param name="fontFinalSize">���ֵ����մ�С</param>
    /// <param name="fontSizeScaleTime">���ִ���ʼ��С�仯�����մ�С��Ҫ��ʱ��</param>
    /// <param name="fontColor">���ֵ���ɫ</param>
    /// <param name="alignment">���뷽ʽ</param>
    /// <param name="lifeTime">��������</param>
    /// <param name="animateTime">���ֿ��Ի��ʱ�䣬������ʱ������ֽ��̶�����������Ӱ�졣</param>
    /// <param name="startFadeTime">��ú����ֿ�ʼ͸��ֱ����ʧ��</param>
    /// <param name="minVelocity">��С��ʼ������ϵͳ����������С��ʼ����֮�����һ����ֵ����Ϊ�ô�������ʾ������</param>
    /// <param name="maxVelocity">����ʼ������ϵͳ����������С��ʼ����֮�����һ����ֵ����Ϊ�ô�������ʾ������</param>
    /// <param name="gravity">����������ܵ�����Ӱ����������޸Ĵ��</param>
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
