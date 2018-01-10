using EasyAIFrame;
using System.Collections.Generic;
using UnityEngine;


public class Tank : Entity
{
    public float maxSpeed = 20;
    public float maxRote = 10;
    public float CheckDis = 10;
    public LayerMask MineLayer;
    public LayerMask TankLayer;

    Rigidbody _rig;
    Collider _col;


    private void Awake()
    {
        _rig = GetComponent<Rigidbody>();
        _col = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Mine"))
        {
            //分数提升
            AgentScore++;
            Destroy(other.gameObject);
            MinesManager.Instance.checkMineNums();
        }
        else if (other.transform.CompareTag("Tank"))
        {
            //分数减少
            AgentScore -= 2;
        }
    }

    protected override float[] SetInputs()
    {
        float[] inputs = new float[4];
        float speed = _rig.velocity.magnitude / maxSpeed;
        float rota = _rig.angularVelocity.magnitude / maxRote;
        float nearTank = getNearestTankAngle();
        float nearMine = getNearestMineAngle();

        inputs[0] = speed;
        inputs[1] = rota;
        inputs[2] = nearTank;
        inputs[3] = nearMine;
        return inputs;
    }

    protected override void GetOutPuts(float[] outputs)
    {
        float speed = outputs[0];
        float rota = outputs[1];
        _rig.velocity = transform.forward * (float)speed * maxSpeed;
        _rig.angularVelocity = transform.up * (float)rota * maxRote;
    }

    /// <summary>
    /// 查找最近的地雷
    /// </summary>
    /// <returns></returns>
    float getNearestMineAngle()
    {
        float res = Random.Range(-1f, 1f);
        List<Collider> mines = new List<Collider>(Physics.OverlapSphere(transform.position, CheckDis, MineLayer));
        if (mines != null && mines.Count != 0)
        {
            mines.Sort((Collider x, Collider y) =>
            {
                return Vector3.Distance(x.transform.position, transform.position).CompareTo(Vector3.Distance(y.transform.position, transform.position));
            });
            Transform targetMine = mines[0].transform;
            Vector3 mineDir = targetMine.position - transform.position;
            targetMinePos = targetMine.position;
            float angle = Vector3.Dot(transform.forward, mineDir);
            //顺逆时针
            int sign = ReturenSign(transform.forward, mineDir);
            res = sign * angle;
        }
        else
        {
            targetMinePos = Vector3.zero;
        }
        return res;
    }

    /// <summary>
    /// 查找最近的tank
    /// </summary>
    /// <returns></returns>
    float getNearestTankAngle()
    {
        float res = 0;
        List<Collider> tanks = new List<Collider>(Physics.OverlapSphere(transform.position, CheckDis, TankLayer));
        tanks.Remove(_col);
        if (tanks != null && tanks.Count != 0)
        {
            tanks.Sort((Collider x, Collider y) =>
            {
                return Vector3.Distance(x.transform.position, transform.position).CompareTo(Vector3.Distance(y.transform.position, transform.position));
            });
            Transform targetTank = tanks[0].transform;
            targetTankPos = targetTank.position;
            Vector3 tankDir = targetTank.position - transform.position;
            float angle = Vector3.Dot(transform.forward, tankDir);
            //顺逆时针
            int sign = ReturenSign(transform.forward, tankDir);
            res = sign * angle;
        }
        else
        {
            targetTankPos = Vector3.zero;
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

    Vector3 targetTankPos = Vector3.zero;
    Vector3 targetMinePos = Vector3.zero;
    private void OnDrawGizmos()
    {
        //画雷线
        if (targetMinePos != Vector3.zero)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, targetMinePos);
        }
        //最近的tank线
        if (targetTankPos != Vector3.zero)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, targetTankPos);
        }
    }

}
