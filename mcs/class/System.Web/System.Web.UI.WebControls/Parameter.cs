//
// System.Web.UI.WebControls.Parameter
//
// Authors:
//	Ben Maurer (bmaurer@users.sourceforge.net)
//
// (C) 2003 Ben Maurer
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

#if NET_2_0
using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Data;

namespace System.Web.UI.WebControls {
	public class Parameter : ICloneable, IStateManager {

		public Parameter () : base ()
		{
		}

		protected Parameter (Parameter original)
		{
			this.DefaultValue = original.DefaultValue;
			this.Direction = original.Direction;
			this.TreatEmptyStringAsNull = original.TreatEmptyStringAsNull;
			this.Type = original.Type;
			this.Name = original.Name;
		}
		
		public Parameter (string name)
		{
			this.Name = name;
		}
		
		public Parameter(string name, TypeCode type) : this (name)
		{
			this.Type = type;
		}
		
		public Parameter (string name, TypeCode type, string defaultValue) : this (name, type)
		{
			this.DefaultValue = defaultValue;
		}
		
		protected virtual Parameter Clone ()
		{
			return new Parameter (this);
		}
		
		protected virtual object Evaluate (Control control)
		{
			return this.DefaultValue;
		}
		
		protected void OnParameterChanged ()
		{
			if (_owner != null)
				_owner.CallOnParameterChanged ();
		}
		
		protected virtual void LoadViewState (object savedState)
		{
			if (savedState == null)
				return;
			
			ViewState.LoadViewState (savedState);
		}
		
		protected virtual object SaveViewState ()
		{
			if (viewState == null)
				return null;
			return viewState.SaveViewState ();
		}
		
		protected virtual void TrackViewState ()
		{
			isTrackingViewState = true;
			if (viewState != null)
				viewState.TrackViewState ();
		}
		
		object ICloneable.Clone ()
		{
			return this.Clone ();
		}
		
		void IStateManager.LoadViewState (object savedState)
		{
			this.LoadViewState (savedState);
		}
		
		object IStateManager.SaveViewState ()
		{
			return this.SaveViewState ();
		}
		
		void IStateManager.TrackViewState ()
		{
			this.TrackViewState ();
		}
		
		bool IStateManager.IsTrackingViewState {
			get { return this.IsTrackingViewState; }
		}
		
		[MonoTODO]
		public override string ToString ()
		{
			return base.ToString ();
		}
		
		public string DefaultValue {
			get {
				return ViewState ["DefaultValue"] as string;
			}
			set {
				
				if (DefaultValue != value) {
					ViewState ["DefaultValue"] = value;
					OnParameterChanged ();
				}
			}
		}
		
		public ParameterDirection Direction {
			get {
				object o = ViewState ["Direction"];
				if (o != null)
					return (ParameterDirection) o;
				
				return ParameterDirection.Input;
			}
			set {
				
				if (Direction != value) {
					ViewState ["Direction"] = value;
					OnParameterChanged ();
				}
			}
		}
		

		public string Name {
			get {
				string s = ViewState ["Name"] as string;
				if (s != null)
					return s;
				
				return "";
			}
			set {
				
				if (Name != value) {
					ViewState ["Name"] = value;
					OnParameterChanged ();
				}
			}
		}

		public bool TreatEmptyStringAsNull {
			get {
				object o = ViewState ["TreatEmptyStringAsNull"];
				if (o != null)
					return (bool) o;
				
				return false;
			}
			set {
				
				if (TreatEmptyStringAsNull != value) {
					ViewState ["TreatEmptyStringAsNull"] = value;
					OnParameterChanged ();
				}
			}
		}
		
		public TypeCode Type {
			get {
				object o = ViewState ["Type"];
				if (o != null)
					return (TypeCode) o;
				
				return TypeCode.Object;
			}
			set {
				
				if (Type != value) {
					ViewState ["Type"] = value;
					OnParameterChanged ();
				}
			}
		}
		
		StateBag viewState;
		
		protected StateBag ViewState {
			get {
				if (viewState == null) {
					viewState = new StateBag ();
					if (IsTrackingViewState)
						viewState.TrackViewState ();
				}
				return viewState;
			}
		}
		
		bool isTrackingViewState = false;
		protected bool IsTrackingViewState {
			get { return isTrackingViewState; }
		}
		
		private ParameterCollection _owner;

		internal void SetOwnerCollection (ParameterCollection own)
		{
			_owner = own;
		}

		internal object ParameterValue {
			get {
				object param = ViewState["ParameterValue"];
				//FIXME: need to do some null string checking magic with TreatEmptyStringAsNull here
				if (param == null)
				{
					param = DefaultValue;
					if (param == null)
					{
						return null;
					}
				}
				return Convert.ChangeType (param, Type);
			}
		}
	}
}
#endif

