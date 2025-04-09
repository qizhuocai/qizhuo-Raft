using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{

    [SerializeField] private bool thrown = false;
    [SerializeField] private float throwForce = 20.0f;
    [SerializeField] private bool canRetract = false;
    [SerializeField] private float retractSpeed = 1.0f;
    [SerializeField] private float autoRetractTime = 5.0f; // 自动拉回的时间，单位：秒

    [SerializeField] private List<Catchable> catched = new List<Catchable>();

    private Rigidbody rb;
    private Collider collider;

    private Vector3 prevPosition;
    private Transform parent;
    private float throwTime; // 记录抛出的时间

    void Start()
    {

        rb = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        parent = transform.parent;

    }
    void Update()
    {

        if (!Player.instance.gameStarted || Player.instance.gamePaused) return;
        if (!thrown)
        {

            transform.localPosition = new Vector3(0.5f, -0.4f, 0.6f);
            transform.localRotation = Quaternion.Euler(180.0f, 90.0f, 0.0f);
            transform.localScale = new Vector3(25.0f, 25.0f, 25.0f);

            if (Input.GetKeyDown(KeyCode.Mouse0)) Throw();
            return;

        }

        // 检查是否达到自动拉回的时间
        if (Time.time - throwTime >= autoRetractTime && !canRetract)
        {
            canRetract = true;
        }

        if (canRetract)
        {
            Retract(new Vector3(Player.instance.transform.position.x, 0.12f, Player.instance.transform.position.z));
        }

        foreach (Catchable catchable in catched)
        {

            catchable.transform.position = transform.position;

        }

    }

    private void Throw()
    {

        thrown = true;
        rb.useGravity = true;
        rb.isKinematic = false;
        rb.AddForce(transform.parent.forward * throwForce, ForceMode.Impulse);
        collider.enabled = true;
        transform.parent = null;
        throwTime = Time.time; // 记录抛出的时间

    }
    private void Retract(Vector3 destination)
    {

        if (!thrown) return;
        if (!canRetract) return;

        Vector3 moveForce = (destination - transform.position).normalized * retractSpeed;
        transform.position = Vector3.MoveTowards(transform.position, destination, retractSpeed * Time.deltaTime);

        // 判断是否接近目标位置
        if (Vector3.Distance(transform.position, destination) < 0.1f)
        {
            FinishRetract();
        }
    }

    private void FinishRetract()
    {

        thrown = false;
        canRetract = false;
        rb.useGravity = false;
        rb.isKinematic = true;
        collider.enabled = false;

        transform.parent = parent;
        transform.localPosition = new Vector3(0.5f, -0.4f, 0.6f);

        foreach (Catchable catchable in catched)
        {

            Inventory.instance.AddItem(catchable.Item, catchable.Amount);
            CatchableManager.instance.RemoveCatchable(catchable);

        }
        catched.Clear();

    }

    void OnTriggerEnter(Collider other)
    {

        if (!thrown) return;

        if (other.tag == "Catchable")
        {

            catched.Add(other.GetComponent<Catchable>());

            other.transform.position = transform.position;
            other.enabled = false;

        }
        else if (other.tag == "Water") canRetract = true;

    }

    void OnCollisionEnter(Collision other)
    {

        if (!thrown) return;

        if (canRetract && (other.gameObject.tag == "Raft" || other.gameObject.tag == "Player")) FinishRetract();

    }

}