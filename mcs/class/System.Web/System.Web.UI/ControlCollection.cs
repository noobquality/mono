//
// System.Web.UI.ControlCollection.cs
//
// Duncan Mak  (duncan@ximian.com)
//
// (C) Ximian, Inc.
//

//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;
using System.Collections;

namespace System.Web.UI {

	public class ControlCollection : ICollection, IEnumerable
	{
		ArrayList list;
		Control owner;
		
		public ControlCollection (Control owner)
		{
			if (owner == null)
				throw new ArgumentException ();

			list = new ArrayList ();
			this.owner = owner;
		}

		internal ControlCollection (Control owner, bool shortList)
		{
			if (owner == null)
				throw new ArgumentException ();

			list = new ArrayList (shortList ? 1 : 0);
			this.owner = owner;
		}

		public int Count {
			get { return list.Count; }
		}

		public bool IsReadOnly {
			get { return list.IsReadOnly; }
		}

		public bool  IsSynchronized {
			get { return list.IsSynchronized; }
		}

		public virtual Control this [int index] {
			get { return list [index] as Control; }
		}

		protected Control Owner {
			get { return owner; }
		}

		public object SyncRoot {
			get { return list.SyncRoot; }
		}

		public virtual void Add (Control child)
		{
			if (child == null)
				throw new ArgumentNullException ();
			if (IsReadOnly)
				throw new HttpException ();

			list.Add (child);
			owner.AddedControl (child, list.Count - 1);
		}

		public virtual void AddAt (int index, Control child)
		{
			if (child == null) // maybe we should check for ! (child is Control)?
				throw new ArgumentNullException ();
			
			if ((index < -1) || (index > Count))
				throw new ArgumentOutOfRangeException ();

			if (IsReadOnly)
				throw new HttpException ();

			if (index == -1){
				Add (child);
			} else {
				list.Insert (index, child);
				owner.AddedControl (child, index);
			}
		}

		public virtual void Clear ()
		{
			list.Clear ();
			if (owner != null)
				owner.ResetChildNames ();
		}

		public virtual bool Contains (Control c)
		{
			return list.Contains (c);
		}

		public void CopyTo (Array array, int index)
		{
			list.CopyTo (array, index);
		}

		public IEnumerator GetEnumerator ()
		{
			return list.GetEnumerator ();
		}

		public virtual int IndexOf (Control c)
		{
			return list.IndexOf (c);
		}

		public virtual void Remove (Control value)
		{
			list.Remove (value);
			owner.RemovedControl (value);
		}

		public virtual void RemoveAt (int index)
		{
			if (IsReadOnly)
				throw new HttpException ();

			Control value = (Control) list [index];
			list.RemoveAt (index);
			owner.RemovedControl (value);
		}
	}
}
