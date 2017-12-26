using AIFrame;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : Entity
{
    public float maxSpeed = 20;
    public float maxRote = 10;
    public float CheckDis = 10;
    public LayerMask MineLayer;

    Rigidbody _rig;

    public double[] Weight;

    private void Awake()
    {
        _rig = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Mine"))
        {
            AgentScore++;
            Destroy(other.gameObject);
            MinesManager.Instance.checkMineNums();
        }
    }

    protected override double[] SetInputs()
    {
        double[] inputs = new double[3];
        double speed = _rig.velocity.magnitude / maxSpeed;
        double rota = _rig.angularVelocity.magnitude / maxRote;
        //double forward = transform.forward.magnitude;
        double nearMine = getNearestMineAngle();

        inputs[0] = speed;
        inputs[1] = rota;
        //inputs[2] = forward;
        inputs[2] = nearMine;
        Debug.Log(_rig.velocity.magnitude + "/" + _rig.angularVelocity.magnitude);
        return inputs;
    }

    protected override void GetOutPuts(double[] outputs)
    {
        Weight = Agent.Genotype.parameters;
        double speed = outputs[0];
        double rota = outputs[1];
        _rig.velocity = transform.forward * (float)speed * maxSpeed;
        _rig.angularVelocity = transform.up * (float)rota * maxRote;
        //Debug.Log(_rig.velocity.magnitude + "/" + _rig.angularVelocity.magnitude);
    }

    float getNearestMineAngle()
    {
        float res = 0;
        List<Collider> mines = new List<Collider>(Physics.OverlapSphere(transform.position, CheckDis, MineLayer));
        if (mines != null && mines.Count != 0)
        {
            mines.Sort((Collider x, Collider y) =>
            {
                return Vector3.Distance(x.transform.position, transform.position).CompareTo(Vector3.Distance(y.transform.position, transform.position));
            });
            Vector3 mineDir = mines[0].transform.position - transform.position;
            float angle = Vector3.Dot(transform.forward, mineDir);
            //顺逆时针
            int sign = ReturenSign(transform.forward, mineDir);
            res = sign * angle;
        }
        return res;
    }

    /// <summary>
    /// 判断两个向量夹角情况，顺逆时针
    /// </summary>
    /// <param name="v1"></param>
    /// <param name="v2"></param>
    /// <returns></returns>
    public static int ReturenSign(Vector3 v1, Vector3 v2)
    {
        if (v1.z * v2.x > v1.x * v2.z)
        {
            return 1;
        }
        else
        {
            return -1;
        }
    }

    public override void OnEvolutionBegin()
    {

    }

    public override void OnEvolutionEnd()
    {

    }

  
}
