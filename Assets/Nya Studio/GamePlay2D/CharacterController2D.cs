using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace meow.GamePlay2D{


    public class CharacterController2D : MonoBehaviour
    {
        public GameObject groundCheckPosition;
        public Vector3 groundCheckSize = new Vector3(0.75f, 0.2f, 1.0f);
        public List<string> jumpableTags = new List<string>();
        public ContactFilter2D contactFilter2D;
        public GameObject debugDirection;
        public GameObject rayDebug;
        public CapsuleCollider2D col;
        public bool isTouchingGround = false;

        private Ray m_ray;

        void Awake()
        {
            col = GetComponent<CapsuleCollider2D>();
        }

        void OnDrawGizmosSelected()
        {
            // Draw a semitransparent blue cube at the transforms position
            if (isTouchingGround)
                Gizmos.color = new Color(0, 1, 0, 0.5f);
            else
                Gizmos.color = new Color(1, 0, 0, 0.5f);
            
            if (groundCheckPosition != null)
            Gizmos.DrawCube(groundCheckPosition.transform.position, groundCheckSize);

            
            //Gizmos.DrawRay(m_ray);
        }

        public bool TestMove(Vector2 _direction, float _offset)
        {
            return TestMove(_direction, _offset, jumpableTags);
        }

        public bool TestMove(Vector2 _direction, float _offset, List<string> _tags)
        {
            List<RaycastHit2D> results = new List<RaycastHit2D>();

            return TestMove(_direction, _offset, _tags, results);
        }

        public bool TestMove(Vector2 _direction, float _offset, List<string> _tags, List<RaycastHit2D> _results)
        {
            Vector2 origin = ((Vector2)transform.position)+col.offset+(_direction*_offset);

            if(debugDirection)
                debugDirection.transform.position = new Vector3(origin.x, origin.y, 0.0f);

            col.enabled = false;
            
            Physics2D.BoxCast(
                origin,
                new Vector2(col.size.x*0.9f, col.size.y*0.9f),
                0.0f,
                Vector2.right,
                contactFilter2D,
                _results,
                0.1f);
            
            foreach(RaycastHit2D hit in _results)
            {
                if (_tags.Contains(hit.collider.gameObject.tag))
                {
                    col.enabled = true;
                    return false;
                }
            }
            
            col.enabled = true;
            return true;
        }

        public bool IsTouchingGround()
        {
            List<RaycastHit2D> results = new List<RaycastHit2D>();

            GroundCheck(results);
            
            if (results.Count > 0)
            {
                foreach(RaycastHit2D hit in results)
                {
                    if (jumpableTags.Contains(hit.collider.gameObject.tag))
                    {
                        return true;
                    }
                }
                return false;
            }
            else
            {
                return false;
            }
        }

        public void GroundCheck(List<RaycastHit2D> _results)
        {
            Physics2D.BoxCast(
                new Vector2(groundCheckPosition.transform.position.x,groundCheckPosition.transform.position.y),
                new Vector2(groundCheckSize.x, groundCheckSize.y),
                0.0f,
                Vector2.right,
                contactFilter2D,
                _results,
                0.1f);
        }

        public RaycastHit2D EdgeHit(Vector3 _position, Vector2 _direction, float _offset)
        {
            Vector2 origin = col.offset;//(direction*offset);
            origin.x += (col.size.x/2) + _offset;
            origin.x *= _direction.x;
            origin += ((Vector2)_position);

            if (rayDebug)
                rayDebug.transform.position = origin;

            RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down);
            m_ray.direction = Vector2.down;
            m_ray.origin = origin;
            return hit;
        }
    }
}