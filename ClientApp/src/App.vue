<template>
	<v-app>
		<Nav :isLoggedIn="isLoggedIn"></Nav>
		<v-main>
			<router-view />
		</v-main>
	</v-app>
</template>

<script>
import service from "./utils/request";
import Nav from "./components/Nav.vue";

export default {
	components: {
		Nav,
	},
	data() {
		return {
			isLoggedIn: false,
		};
	},
	watch: {
		$route() {
			this.isAuthenticated();
		},
	},
	methods: {
		isAuthenticated: function () {
			service
				.get("/auth/is-authenticated")
				.then(() => (this.isLoggedIn = true))
				.catch(() => (this.isLoggedIn = false));
		},
	},
};
</script>

<style lang="scss">
#app {
	font-family: Avenir, Helvetica, Arial, sans-serif;
	-webkit-font-smoothing: antialiased;
	-moz-osx-font-smoothing: grayscale;
	text-align: center;
	color: #2c3e50;
}

#nav {
	padding: 30px;

	a {
		font-weight: bold;
		color: #2c3e50;

		&.router-link-exact-active {
			color: #42b983;
		}
	}
}
.v-main__wrap {
	display: flex;
	justify-content: center;
	align-items: center;
}
</style>
