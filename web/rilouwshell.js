"use strict";
const RilouwShell = (function(){

  const KEY_ENTER = 13;
  const KEY_SPACE = 32;

  const $ = (element) => ({
    all: (element || document.body).querySelectorAll.bind(element || document.body),
    one: (element || document.body).querySelector.bind(element || document.body),
    byId: document.getElementById.bind(document)
  });
  Object.assign($, $());

  function handleKeys(keys, cont) {
    return (e) => {
      if(keys.includes(e.which)) {
        e.stopPropagation();
        e.preventDefault();
        return cont();
      }
      return true;
    };
  }

  function getAttributes(element) {
    return Object.assign({}, ...Array.from(element.attributes, ({name, value}) => ({[name]: value})));
  }

  class RilouwElement extends HTMLElement {
    constructor(name) {
      super();
      const template = $.byId(`${name}-template`);
      const attributes = Object.assign({body: this.innerHTML}, getAttributes(this));
      const content = this.processTemplate(template, attributes);
      this.innerHTML = "";
      this.appendChild(content);
      this.init(attributes);
    }

    processTemplate(template, attributes) {
      const root = template.content.cloneNode(true);
      Page.evalTemplate(root, attributes);
      return root;
    }

    init() {}
  }

  class RilouwCheckbox extends RilouwElement {
    constructor() {
      super('rilouw-checkbox');
    }

    init(attributes) {
      this.value = attributes['default-value'] === 'true';
      this.updateDom();
      const label = $(this).one('label');
      label.addEventListener('click', (e) => {
        e.stopPropagation();
        e.preventDefault();
        this.toggle();
      });
      label.addEventListener('keydown', handleKeys([KEY_SPACE, KEY_ENTER], () => this.toggle()));
    }

    toggle() {
      this.value = !this.value;
      this.updateDom();
    }

    updateDom() {
      $(this).one('input[type="checkbox"]').checked = this.value;
      $(this).all('option.selected').forEach(o => o.classList.remove('selected'));
      $(this).one(`option[value="${this.value}"]`).classList.add('selected');
    }
  }

  class RilouwSelector extends RilouwElement {
    constructor() {
      super('rilouw-selector');
    }

    processTemplate(template, attributes) {
      const root = RilouwElement.prototype.processTemplate.call(this, template, attributes);
      this.optionValues = JSON.parse(attributes['values']);
      Object.entries(this.optionValues).forEach(([key, text]) => {
        $(root).one('datalist').appendChild(Page.createElementFromJson({
          tag: 'option',
          text: text,
          attrs: { tabindex: 0, role: 'button', value: key }
        }));
      });
      return root;
    }

    init(attributes) {
      const button = $(this).one('button');
      const modal = $(this).one('aside.modal');

      const showModal = () => {
        modal.classList.remove('hidden');
        setTimeout(() => $(modal).one('datalist option').focus(), 250);
        return true;
      };

      const hideModal = () => {
        modal.classList.add('hidden');
        setTimeout(() => button.focus(), 250);
        return true;
      };

      button.addEventListener('click', showModal);

      $(modal).all('datalist option').forEach(option => {
        const selectOption = () => {
          this.select(option.value);
          hideModal();
        };
        option.addEventListener('click', selectOption);
        option.addEventListener('keydown', handleKeys([KEY_SPACE, KEY_ENTER], selectOption));
      });

      this.select(attributes['default-value']);
    }

    select(key) {
      $(this).all('option').forEach((option) => {
        option.classList.remove('selected');
        if(option.value === key) {
          option.classList.add('selected');
        }
      });
      $(this).one('input[type="hidden"]').value = key;
      $(this).one('button').innerHTML = this.optionValues[key];
    }
  }

  function init() {
    customElements.define('rilouw-checkbox', RilouwCheckbox);
    customElements.define('rilouw-selector', RilouwSelector);
  }

  return { init };
}());
