<template>
    <img src="../../../assets/emojis/{{angry}}.png" />
</template>

<script>

    export default {
        watch: {
            '$route': 'fetchItems'
        },
        methods: {
            fetchItems() {
                this.items = [];
                this.type = this.$route.params.type;
                let initial_ids = [1, 13, 14];
                for (let i in initial_ids) {

                    let id = initial_ids[i];
                    let address = 'https://swapi.co/api/' + this.type + '/' + id;
                    fetch(address, {
                        method: 'GET',
                    })
                    .then(response => response.json())
                    .then(json => this.items.push(json)) ;
                }
            }
        },
        created() {
            this.fetchItems();
        },
        components: {
            Item
        }
    }
</script>