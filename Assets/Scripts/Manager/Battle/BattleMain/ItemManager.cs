﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// アイテムを管理するマネージャ。
/// </summary>
public class ItemManager : BattleSingletonMonoBehavior<ItemManager>
{
    public const string HOLDER_NAME = "[ItemHolder]";

    #region Field Inspector

    /// <summary>
    /// アイテムオブジェクトを保持する。
    /// </summary>
    [SerializeField]
    private Transform m_ItemHolder;

    /// <summary>
    /// アイテムのプレハブ群。
    /// </summary>
    [SerializeField]
    private ItemController[] m_ItemPrefabs;

    /// <summary>
    /// アイテムに適用する軌道パラメータ。
    /// </summary>
    [SerializeField]
    private BulletOrbitalParam m_ItemOrbitalParam;

    /// <summary>
    /// アイテムの吸引強度。
    /// </summary>
    [SerializeField]
    private float m_ItemAttractRate;

    /// <summary>
    /// STANDBY状態のアイテムを保持するリスト。
    /// </summary>
    [SerializeField]
    private List<ItemController> m_StandbyItems;

    /// <summary>
    /// UPDATE状態のアイテムを保持するリスト。
    /// </summary>
    [SerializeField]
    private List<ItemController> m_UpdateItems;

    /// <summary>
    /// POOL状態のアイテムを保持するリスト。
    /// </summary>
    [SerializeField]
    private List<ItemController> m_PoolItems;

    /// <summary>
    /// UPDATE状態に遷移するアイテムのリスト。
    /// </summary>
    private List<ItemController> m_GotoUpdateItems;

    /// <summary>
    /// POOL状態に遷移するアイテムのリスト。
    /// </summary>
    private List<ItemController> m_GotoPoolItems;

    #endregion

    private Dictionary<E_ITEM_TYPE, ItemController> m_ItemPrefabCache;


    #region Get

    /// <summary>
    /// アイテムの吸引強度を取得する。
    /// </summary>
    public float GetItemAttractRate()
    {
        return m_ItemAttractRate;
    }

    /// <summary>
    /// STANDBY状態のアイテムを保持するリストを取得する。
    /// </summary>
    public List<ItemController> GetStandbyItems()
    {
        return m_StandbyItems;
    }

    /// <summary>
    /// UPDATE状態のアイテムを保持するリストを取得する。
    /// </summary>
    public List<ItemController> GetUpdateItems()
    {
        return m_UpdateItems;
    }

    /// <summary>
    /// POOL状態のアイテムを保持するリストを取得する。
    /// </summary>
    public List<ItemController> GetPoolItems()
    {
        return m_PoolItems;
    }

    #endregion



    protected override void OnAwake()
    {
        base.OnAwake();

        m_StandbyItems = new List<ItemController>();
        m_UpdateItems = new List<ItemController>();
        m_PoolItems = new List<ItemController>();
        m_GotoUpdateItems = new List<ItemController>();
        m_GotoPoolItems = new List<ItemController>();

        m_ItemPrefabCache = new Dictionary<E_ITEM_TYPE, ItemController>();
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
    }

    public override void OnFinalize()
    {
        base.OnFinalize();
        m_StandbyItems.Clear();
        m_UpdateItems.Clear();
        m_PoolItems.Clear();
    }

    public override void OnStart()
    {
        base.OnStart();

        if (StageManager.Instance != null && StageManager.Instance.GetItemHolder() != null)
        {
            m_ItemHolder = StageManager.Instance.GetItemHolder().transform;
        }
        else if (m_ItemHolder == null)
        {
            var obj = new GameObject(HOLDER_NAME);
            obj.transform.position = Vector3.zero;
            m_ItemHolder = obj.transform;
        }
    }

    public override void OnUpdate()
    {
        // Start処理
        foreach (var bullet in m_StandbyItems)
        {
            if (bullet == null)
            {
                continue;
            }

            bullet.OnStart();
            m_GotoUpdateItems.Add(bullet);
        }

        GotoUpdateFromStandby();

        // Update処理
        foreach (var bullet in m_UpdateItems)
        {
            if (bullet == null)
            {
                continue;
            }

            bullet.OnUpdate();
        }
    }

    public override void OnLateUpdate()
    {
        // LateUpdate処理
        foreach (var bullet in m_UpdateItems)
        {
            if (bullet == null)
            {
                continue;
            }

            bullet.OnLateUpdate();
        }

        GotoPoolFromUpdate();
    }



    /// <summary>
    /// UPDATE状態にする。
    /// </summary>
    private void GotoUpdateFromStandby()
    {
        int count = m_GotoUpdateItems.Count;

        for (int i = 0; i < count; i++)
        {
            int idx = count - i - 1;
            var bullet = m_GotoUpdateItems[idx];
            m_GotoUpdateItems.RemoveAt(idx);
            m_StandbyItems.Remove(bullet);
            m_UpdateItems.Add(bullet);
            bullet.SetItemCycle(ItemController.E_ITEM_CYCLE.UPDATE);
        }

        m_GotoUpdateItems.Clear();
    }

    /// <summary>
    /// POOL状態にする。
    /// </summary>
    private void GotoPoolFromUpdate()
    {
        int count = m_GotoPoolItems.Count;

        for (int i = 0; i < count; i++)
        {
            int idx = count - i - 1;
            var bullet = m_GotoPoolItems[idx];
            bullet.SetItemCycle(ItemController.E_ITEM_CYCLE.POOLED);
            m_GotoPoolItems.RemoveAt(idx);
            m_UpdateItems.Remove(bullet);
            m_PoolItems.Add(bullet);
        }

        m_GotoPoolItems.Clear();
    }

    /// <summary>
    /// アイテムをSTANDBY状態にして制御下に入れる。
    /// </summary>
    public void CheckStandbyItem(ItemController item)
    {
        if (item == null || !m_PoolItems.Contains(item))
        {
            Debug.LogError("指定されたアイテムを追加できませんでした。");
            return;
        }

        m_PoolItems.Remove(item);
        m_StandbyItems.Add(item);
        item.gameObject.SetActive(true);
        item.SetItemCycle(ItemController.E_ITEM_CYCLE.STANDBY_UPDATE);
        item.OnInitialize();
    }

    /// <summary>
    /// 指定したアイテムを制御から外すためにチェックする。
    /// </summary>
    public void CheckPoolItem(ItemController item)
    {
        if (item == null || m_GotoPoolItems.Contains(item))
        {
            Debug.LogError("指定したアイテムを削除できませんでした。");
            return;
        }

        item.SetItemCycle(ItemController.E_ITEM_CYCLE.STANDBY_POOL);
        item.OnFinalize();
        m_GotoPoolItems.Add(item);
        item.gameObject.SetActive(false);
    }

    /// <summary>
    /// プールからアイテムを取得する。
    /// 足りなければ生成する。
    /// </summary>
    /// <param name="itemPrefab">取得や生成の情報源となるアイテムのプレハブ</param>
    public ItemController GetPoolingItem(ItemController itemPrefab)
    {
        if (itemPrefab == null)
        {
            return null;
        }

        E_ITEM_TYPE itemType = itemPrefab.GetItemType();
        ItemController item = null;

        foreach (var i in m_PoolItems)
        {
            if (i != null && i.GetItemType() == itemType)
            {
                item = i;
                break;
            }
        }

        if (item == null)
        {
            item = Instantiate(itemPrefab);
            item.transform.SetParent(m_ItemHolder);
            m_PoolItems.Add(item);
        }

        return item;
    }

    /// <summary>
    /// アイテムの種類を指定してアイテムプレハブを取得する。
    /// なければnullを返す。
    /// </summary>
    public ItemController GetItemPrefabFromItemType(E_ITEM_TYPE itemType)
    {
        // キャッシュされていれば、それを返す
        if (m_ItemPrefabCache.ContainsKey(itemType))
        {
            var item = m_ItemPrefabCache[itemType];
            if (item != null)
            {
                return item;
            }
        }

        foreach(var item in m_ItemPrefabs)
        {
            if (item == null)
            {
                continue;
            }

            if (item.GetItemType() == itemType)
            {
                m_ItemPrefabCache.Add(itemType, item);
                return item;
            }
        }

        return null;
    }

    /// <summary>
    /// 指定した座標から指定した情報でアイテムを生成する。
    /// </summary>
    /// 
    /// <param name="position">生成座標</param>
    /// <param name="param">アイテムの生成情報</param>
    public void CreateItem(Vector3 position, ItemCreateParam param)
    {
        foreach(var spreadParam in param.ItemSpreadParams)
        {
            var prefab = GetItemPrefabFromItemType(spreadParam.ItemType);
            if (prefab == null)
            {
                continue;
            }

            var item = GetPoolingItem(prefab);
            item.transform.SetParent(m_ItemHolder);

            if (spreadParam.ItemType == param.CenterCreateItemType)
            {
                item.SetPosition(position);
            } else
            {
                float r = spreadParam.SpreadRadius;
                float x = Random.Range(-r, r);
                float zR = Mathf.Sqrt(r * r - x * x);
                float z = Random.Range(-zR, zR);
                item.SetPosition(position + new Vector3(x, 0, z));
            }

            item.ChangeOrbital(m_ItemOrbitalParam);
            CheckStandbyItem(item);
        }
    }

    /// <summary>
    /// 全てのアイテムを吸引状態にする。
    /// </summary>
    public void AttractAllItem()
    {
        foreach(var item in m_StandbyItems)
        {
            item.AttractPlayer();
        }

        foreach(var item in m_UpdateItems)
        {
            item.AttractPlayer();
        }
    }

    public void OnAttractAction(InputManager.E_INPUT_STATE state)
    {
        AttractAllItem();
    }
}
